using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WindowsControls.Aysnc
{
    public class ExclusiveExecutionMachine : IObservable<bool>
    {
        public event EventHandler? IsEnteredChanged;

        public bool IsEntered { get; private set; }

        IObserver<bool>[]? observers;

        public ulong token;

        object notificationLockObj = new object();

        SemaphoreSlim invocationSemaphore = new SemaphoreSlim(1);

        bool pushValueAtSubscribe;


        public ExclusiveExecutionMachine(bool pushValueAtSubscribe)
        {
            this.pushValueAtSubscribe = pushValueAtSubscribe;
        }

        public IDisposable Subscribe(IObserver<bool> observer)
        {
            var disposer = new SubscribeDisposer(this, observer);

            IObserver<bool>[]? previousObservers = null;
            IObserver<bool>[]? nextObservers = null;

            do
            {
                previousObservers = observers;

                var nextLength = previousObservers is null ? 1 : previousObservers.Length + 1;
                nextObservers = new IObserver<bool>[nextLength];

                previousObservers?.CopyTo(nextObservers, 0);
                nextObservers[nextLength - 1] = observer;

            } while (!ReferenceEquals(Interlocked.CompareExchange(ref observers, nextObservers, previousObservers), previousObservers));

            if (pushValueAtSubscribe)
            {
                observer.OnNext(IsEntered);
            }

            return disposer;
        }

        public async Task<ExclusiveExecutionContext> EnterAsync()
        {
            await invocationSemaphore.WaitAsync();
            try
            {
                IsEntered = true;
                token += 10;

                // ロック解除の通知中に別スレッドから新たなロックがとられたときに通知が前後しないようにするための排他制御
                lock (notificationLockObj)
                {
                    IsEnteredChanged?.Invoke(this, EventArgs.Empty);

                    var workList = observers;

                    if (workList is { })
                    {
                        foreach (var observer in workList)
                        {
                            observer.OnNext(true);
                        }
                    }
                }

                return new ExclusiveExecutionContext(this, token);
            }
            catch (Exception ex) when (ReleaseForException())
            {
                throw ex;
            }

            bool ReleaseForException()
            {
                invocationSemaphore.Release();
                return false;
            }
        }

        public async static Task<CompositeExclusiveExecutionContext> EnterAllAsync(params ExclusiveExecutionMachine[] invocationMachines)
        {
            return new CompositeExclusiveExecutionContext(await Task.WhenAll(invocationMachines.Select(v => v.EnterAsync())));
        }

        public struct CompositeExclusiveExecutionContext : IDisposable
        {
            ExclusiveExecutionContext[]? contexts;

            public CompositeExclusiveExecutionContext(ExclusiveExecutionContext[] contexts)
            {
                this.contexts = contexts ?? throw new ArgumentNullException(nameof(contexts));
            }

            public void Dispose()
            {
                var workContexts = Interlocked.Exchange(ref contexts, null);

                if (workContexts is { })
                {
                    foreach (var context in workContexts)
                    {
                        context.Dispose();
                    }
                }
            }
        }

        public struct ExclusiveExecutionContext : IDisposable
        {
            ExclusiveExecutionMachine owner;
            ulong token;

            public ExclusiveExecutionContext(ExclusiveExecutionMachine owner, ulong token)
            {
                this.owner = owner ?? throw new ArgumentNullException(nameof(owner));
                this.token = token;
            }

            public void Dispose()
            {
                if (owner.token != token)
                {
                    throw new InvalidOperationException();
                }

                // ロック解除の通知中に別スレッドから新たなロックがとられたときに通知が前後しないようにするための排他制御
                lock (owner.notificationLockObj)
                {
                    owner.token -= 1;
                    owner.IsEntered = false;
                    owner.invocationSemaphore.Release();

                    owner.IsEnteredChanged?.Invoke(owner, EventArgs.Empty);

                    var workList = owner.observers;

                    if (workList is { })
                    {
                        foreach (var observer in workList)
                        {
                            observer.OnNext(false);
                        }
                    }
                }
            }
        }

        class SubscribeDisposer : IDisposable
        {
            ExclusiveExecutionMachine owner;
            IObserver<bool>? observer;

            public SubscribeDisposer(ExclusiveExecutionMachine owner, IObserver<bool> observer)
            {
                this.owner = owner ?? throw new ArgumentNullException(nameof(owner));
                this.observer = observer ?? throw new ArgumentNullException(nameof(observer));
            }

            public void Dispose()
            {
                var removeObject = Interlocked.Exchange(ref observer, null);

                if (removeObject is { })
                {
                    IObserver<bool>[]? previousObservers = null;
                    IObserver<bool>[]? nextObservers = null;

                    do
                    {
                        previousObservers = owner.observers;

                        if (previousObservers is null)
                        {
                            return;
                        }

                        var removeIndex = Array.IndexOf(previousObservers, removeObject);

                        if (removeIndex < 0)
                        {
                            return;
                        }

                        nextObservers = new IObserver<bool>[previousObservers.Length - 1];

                        if (removeIndex == 0)
                        {
                            Array.Copy(previousObservers, 1, nextObservers, 0, previousObservers.Length - 1);
                        }
                        else if (removeIndex == previousObservers.Length - 1)
                        {
                            Array.Copy(previousObservers, 0, nextObservers, 0, previousObservers.Length - 1);
                        }
                        else
                        {
                            Array.Copy(previousObservers, 0, nextObservers, 0, removeIndex);
                            Array.Copy(previousObservers, removeIndex + 1, nextObservers, removeIndex, previousObservers.Length - removeIndex - 1);
                        }

                    } while (!ReferenceEquals(Interlocked.CompareExchange(ref owner.observers, nextObservers, previousObservers), previousObservers));
                }
            }
        }

    }
}
