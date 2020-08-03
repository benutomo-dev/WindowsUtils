using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace WindowsControls.Aysnc.Forms
{
    public static class ExclusiveExecutionMachineBinding
    {
        private static ConditionalWeakTable<ExclusiveExecutionMachine, List<WeakReference<Control>?>> machineToControlTable = new ConditionalWeakTable<ExclusiveExecutionMachine, List<WeakReference<Control>?>>();
        private static ConditionalWeakTable<Control, List<WeakReference<ExclusiveExecutionMachine>?>> controlToMachineTable = new ConditionalWeakTable<Control, List<WeakReference<ExclusiveExecutionMachine>?>>();

        private static object lockObj = new object();

        public static void AddBindingExclusiveExecutionMachines(this Control control, IEnumerable<ExclusiveExecutionMachine> exclusiveExecutionMachines)
        {
            if (control is null) throw new ArgumentNullException(nameof(control));
            if (exclusiveExecutionMachines is null) throw new ArgumentNullException(nameof(exclusiveExecutionMachines));


            int additionalMachinesCount;
            if (exclusiveExecutionMachines is System.Collections.ICollection collection)
            {
                additionalMachinesCount = collection.Count;
            }
            else
            {
                var tempList = exclusiveExecutionMachines.ToList();
                additionalMachinesCount = tempList.Count;
                exclusiveExecutionMachines = tempList;
            }

            if (additionalMachinesCount == 0)
            {
                return;
            }

            lock (lockObj)
            {
                UpdateMachineToControlTable(control, exclusiveExecutionMachines);
                UpdateControlToMachineTable(control, additionalMachinesCount, exclusiveExecutionMachines);
            }

            return;

            // machineToControlTableの更新とイベント監視登録(必要時)
            static void UpdateMachineToControlTable(Control control, IEnumerable<ExclusiveExecutionMachine> exclusiveExecutionMachines)
            {
                foreach (var machine in exclusiveExecutionMachines)
                {
                    var additionalControlReference = new WeakReference<Control>(control);

                    if (machineToControlTable.TryGetValue(machine, out var controls))
                    {
                        for (int i = 0; i < controls.Count; i++)
                        {
                            if (!controls[i]?.TryGetTarget(out _) ?? true)
                            {
                                controls[i] = additionalControlReference;
                                goto END_OF_ADD_CONTROL;
                            }
                        }
                        controls.Capacity = Math.Max(controls.Count + 1, controls.Capacity);
                        controls.Add(additionalControlReference);

                    END_OF_ADD_CONTROL:
                        ;
                    }
                    else
                    {
                        controls = new List<WeakReference<Control>?>(1);
                        controls.Add(additionalControlReference);
                        machineToControlTable.Add(machine, controls);

                        // 排他実行機構が初めて登録される場合は排他実行機構に対するイベント監視も登録
                        machine.IsEnteredChanged += ExclusiveExecutionMachine_IsEnteredChanged;
                    }
                }
            }


            // controlToMachineTableの更新とイベント監視登録(必要時)
            static void UpdateControlToMachineTable(Control control, int additionalMachinesCount, IEnumerable< ExclusiveExecutionMachine> exclusiveExecutionMachines)
            {
                if (controlToMachineTable.TryGetValue(control, out var machines))
                {
                    using (var additionalMachinesEnumerator = exclusiveExecutionMachines.GetEnumerator())
                    {
                        var addedCount = 0;

                        for (int i = 0; i < machines.Count; i++)
                        {
                            if (!machines[i]?.TryGetTarget(out _) ?? true)
                            {
                                if (additionalMachinesEnumerator.MoveNext())
                                {
                                    machines[i] = new WeakReference<ExclusiveExecutionMachine>(additionalMachinesEnumerator.Current);
                                    addedCount++;
                                }
                                else
                                {
                                    goto END_OF_ADD_MACHINE;
                                }
                            }
                        }

                        if (addedCount < additionalMachinesCount)
                        {
                            machines.Capacity = Math.Max(machines.Count + (additionalMachinesCount - addedCount), machines.Capacity);
                        }

                        while (additionalMachinesEnumerator.MoveNext())
                        {
                            machines.Add(new WeakReference<ExclusiveExecutionMachine>(additionalMachinesEnumerator.Current));
                        }

                    END_OF_ADD_MACHINE:
                        ;
                    }
                }
                else
                {
                    var list = new List<WeakReference<ExclusiveExecutionMachine>?>(additionalMachinesCount);
                    list.AddRange(exclusiveExecutionMachines.Select(v => new WeakReference<ExclusiveExecutionMachine>(v)));
                    controlToMachineTable.Add(control, list);

                    // コントロールが初めて登録される場合はコントロールに対するイベント監視も登録
                    control.HandleCreated += Control_HandleCreated;
                }
            }
        }

        public static void AddBindingExclusiveExecutionMachines(this Control control, params ExclusiveExecutionMachine[] exclusiveExecutionMachines)
        {
            if (control is null) throw new ArgumentNullException(nameof(control));
            if (exclusiveExecutionMachines is null) throw new ArgumentNullException(nameof(exclusiveExecutionMachines));

            AddBindingExclusiveExecutionMachines(control, (IEnumerable<ExclusiveExecutionMachine>)exclusiveExecutionMachines);
        }

        public static void AddBindingExclusiveExecutionMachine(this Control control, ExclusiveExecutionMachine exclusiveExecutionMachine)
        {
            if (control is null) throw new ArgumentNullException(nameof(control));
            if (exclusiveExecutionMachine is null) throw new ArgumentNullException(nameof(exclusiveExecutionMachine));

            AddBindingExclusiveExecutionMachines(control, (IEnumerable<ExclusiveExecutionMachine>)new[] { exclusiveExecutionMachine });

            UpdateControlEnabledValue(control);
        }


        public static void RemoveBindingExclusiveExecutionMachines(this Control control, IEnumerable<ExclusiveExecutionMachine> exclusiveExecutionMachines)
        {
            if (control is null) throw new ArgumentNullException(nameof(control));
            if (exclusiveExecutionMachines is null) throw new ArgumentNullException(nameof(exclusiveExecutionMachines));

            int removeMachinesCount;
            if (exclusiveExecutionMachines is System.Collections.ICollection collection)
            {
                removeMachinesCount = collection.Count;
            }
            else
            {
                var tempList = exclusiveExecutionMachines.ToList();
                removeMachinesCount = tempList.Count;
                exclusiveExecutionMachines = tempList;
            }

            if (removeMachinesCount == 0)
            {
                return;
            }

            lock (lockObj)
            {
                UpdateMachineToControlTable(control, exclusiveExecutionMachines);
                UpdateControlToMachineTable(control, exclusiveExecutionMachines);
            }

            return;

            // machineToControlTableの更新とイベント監視解除(必要時)
            static void UpdateMachineToControlTable(Control removeControl, IEnumerable<ExclusiveExecutionMachine> exclusiveExecutionMachines)
            {
                foreach (var machine in exclusiveExecutionMachines)
                {
                    if (!machineToControlTable.TryGetValue(machine, out var controls))
                    {
                        continue;
                    }

                    var foundValidEntry = false;
                    var foundRemoveControl = false;

                    for (int i = 0; i < controls.Count && (!foundValidEntry || !foundRemoveControl); i++)
                    {
                        var controlReference = controls[i];

                        if (controlReference is null)
                        {
                            continue;
                        }

                        if (controlReference.TryGetTarget(out var control))
                        {
                            if (ReferenceEquals(control, removeControl))
                            {
                                controls[i] = null;
                                foundRemoveControl = true;
                            }
                            else
                            {
                                foundValidEntry = true;
                            }
                        }
                        else
                        {
                            controls[i] = null;
                        }
                    }

                    if (foundValidEntry)
                    {
                        return;
                    }

                    // 削除後にcontrolsは空になった
                    Debug.Assert(controls.All(v => v is null));

                    machine.IsEnteredChanged -= ExclusiveExecutionMachine_IsEnteredChanged;
                    machineToControlTable.Remove(machine);
                }
            }


            // controlToMachineTableの更新とイベント監視解除(必要時)
            static void UpdateControlToMachineTable(Control control, IEnumerable<ExclusiveExecutionMachine> exclusiveExecutionMachines)
            {
                if (!controlToMachineTable.TryGetValue(control, out var machines))
                {
                    return;
                }

                var removeList = exclusiveExecutionMachines.Cast<ExclusiveExecutionMachine?>().ToList();

                var foundValidEntry = false;
                var removeCount = 0;

                for (int i = 0; i < machines.Count && (!foundValidEntry || removeCount < removeList.Count); i++)
                {
                    var currentMachineReference = machines[i];

                    if (currentMachineReference is null)
                    {
                        continue;
                    }

                    if (currentMachineReference.TryGetTarget(out var currentMachine))
                    {
                        var indexInRemoveList = removeList.IndexOf(currentMachine);

                        if (indexInRemoveList >= 0)
                        {
                            machines[i] = null;
                            removeList[indexInRemoveList] = null;
                            removeCount++;
                        }
                        else
                        {
                            // 元々存在し、かつ今回削除されなかったエントリが見つかった
                            // このループを抜けた後もmachinesは空になっていない。
                            foundValidEntry = true;
                        }
                    }
                    else
                    {
                        machines[i] = null;
                    }
                }

                if (foundValidEntry)
                {
                    return;
                }

                // 削除後にmachinesは空になった
                Debug.Assert(machines.All(v => v is null));

                control.HandleCreated -= Control_HandleCreated;
                controlToMachineTable.Remove(control);

                // バインディングから切り離されたContolのEneableはtrueで確定しておく
                try
                {
                    if (IsValidState(control))
                    {
                        if (control.InvokeRequired)
                        {
                            control.BeginInvoke(new Action(() => control.Enabled = true));
                        }
                        else
                        {
                            control.Enabled = true;
                        }
                    }
                }
                catch (ObjectDisposedException)
                {
                }
            }
        }

        public static void RemoveBindingExclusiveExecutionMachines(this Control control, ExclusiveExecutionMachine[] exclusiveExecutionMachines)
        {
            if (control is null) throw new ArgumentNullException(nameof(control));
            if (exclusiveExecutionMachines is null) throw new ArgumentNullException(nameof(exclusiveExecutionMachines));

            RemoveBindingExclusiveExecutionMachines(control, (IEnumerable<ExclusiveExecutionMachine>)exclusiveExecutionMachines);
        }

        public static void RemoveBindingExclusiveExecutionMachine(this Control control, ExclusiveExecutionMachine exclusiveExecutionMachine)
        {
            if (control is null) throw new ArgumentNullException(nameof(control));
            if (exclusiveExecutionMachine is null) throw new ArgumentNullException(nameof(exclusiveExecutionMachine));

            RemoveBindingExclusiveExecutionMachines(control, (IEnumerable<ExclusiveExecutionMachine>)new[] { exclusiveExecutionMachine  });
        }

        private static void Control_HandleCreated(object? sender, EventArgs e)
        {
            if (!(sender is Control control))
            {
                return;
            }

            UpdateControlEnabledValue(control);
        }

        private static void ExclusiveExecutionMachine_IsEnteredChanged(object? src, EventArgs args)
        {
            if (!(src is ExclusiveExecutionMachine exclusiveExecutionMachine))
            {
                return;
            }

            if (!machineToControlTable.TryGetValue(exclusiveExecutionMachine, out var contols))
            {
                Debug.Fail("状態管理が正常であれば原則生じないはず。");
                return;
            }

            // 状態が更新された排他実行機構にバインディングされている有効なControl全てを更新する
            for (int i = 0; i < contols.Count; i++)
            {
                var controlReference = contols[i];

                if (controlReference is { })
                {
                    if (controlReference.TryGetTarget(out var control))
                    {
                        UpdateControlEnabledValue(control);
                    }
                    else
                    {
                        contols[i] = null;
                    }
                }
            }
        }

        private static void UpdateControlEnabledValue(Control? control)
        {
            if (control is null || !IsValidState(control))
            {
                return;
            }

            if (control.InvokeRequired)
            {
                control.BeginInvoke(new Action<Control>(UpdateControlEnabledValueCore), control);
            }
            else
            {
                UpdateControlEnabledValueCore(control);
            }

            static void UpdateControlEnabledValueCore(Control control)
            {
                if (!controlToMachineTable.TryGetValue(control, out var bindingMachines))
                {
                    Debug.Fail("状態管理が正常であれば原則生じないはず。");
                    return;
                }

                var currentControlEnabledValue = true;

                for (int i = 0; i < bindingMachines.Count; i++)
                {
                    var bindingMachineReference = bindingMachines[i];
                    if (bindingMachineReference is { })
                    {
                        if (bindingMachineReference.TryGetTarget(out var bindingMachine))
                        {
                            if (bindingMachine.IsEntered)
                            {
                                // 一つでもIsEnteredがtrueとなる排他実行機構が存在する場合はバインディングされているControlを無効化する
                                currentControlEnabledValue = false;
                                break;
                            }
                        }
                        else
                        {
                            bindingMachines[i] = null;
                        }
                    }
                }

                control.Enabled = currentControlEnabledValue;
            }
        }

        private static bool IsValidState(Control? control) => !(control is null || !control.IsHandleCreated || control.Disposing || control.IsDisposed);
    }
}
