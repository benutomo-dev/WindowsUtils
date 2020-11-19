using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsControls
{
    public static class StockIcons
    {
        public static Icon DocNoAssocLarge => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_DOCNOASSOC, SHGSI.SHGSI_LARGEICON)).Value;
        public static Icon DocAssocLarge => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_DOCASSOC, SHGSI.SHGSI_LARGEICON)).Value;
        public static Icon ApplicationLarge => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_APPLICATION, SHGSI.SHGSI_LARGEICON)).Value;
        public static Icon FolderLarge => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_FOLDER, SHGSI.SHGSI_LARGEICON)).Value;
        public static Icon FolderOpenLarge => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_FOLDEROPEN, SHGSI.SHGSI_LARGEICON)).Value;
        public static Icon Drive525Large => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_DRIVE525, SHGSI.SHGSI_LARGEICON)).Value;
        public static Icon Drive35Large => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_DRIVE35, SHGSI.SHGSI_LARGEICON)).Value;
        public static Icon DriveRemoveLarge => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_DRIVEREMOVE, SHGSI.SHGSI_LARGEICON)).Value;
        public static Icon DriveFixedLarge => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_DRIVEFIXED, SHGSI.SHGSI_LARGEICON)).Value;
        public static Icon DriveNetLarge => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_DRIVENET, SHGSI.SHGSI_LARGEICON)).Value;
        public static Icon DriveNetDisabledLarge => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_DRIVENETDISABLED, SHGSI.SHGSI_LARGEICON)).Value;
        public static Icon DriveCdLarge => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_DRIVECD, SHGSI.SHGSI_LARGEICON)).Value;
        public static Icon DriveRamLarge => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_DRIVERAM, SHGSI.SHGSI_LARGEICON)).Value;
        public static Icon WorldLarge => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_WORLD, SHGSI.SHGSI_LARGEICON)).Value;
        public static Icon ServerLarge => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_SERVER, SHGSI.SHGSI_LARGEICON)).Value;
        public static Icon PrinterLarge => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_PRINTER, SHGSI.SHGSI_LARGEICON)).Value;
        public static Icon MyNetworkLarge => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_MYNETWORK, SHGSI.SHGSI_LARGEICON)).Value;
        public static Icon FindLarge => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_FIND, SHGSI.SHGSI_LARGEICON)).Value;
        public static Icon HelpLarge => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_HELP, SHGSI.SHGSI_LARGEICON)).Value;
        public static Icon ShareLarge => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_SHARE, SHGSI.SHGSI_LARGEICON)).Value;
        public static Icon LinkLarge => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_LINK, SHGSI.SHGSI_LARGEICON)).Value;
        public static Icon SlowFileLarge => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_SLOWFILE, SHGSI.SHGSI_LARGEICON)).Value;
        public static Icon RecyclerLarge => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_RECYCLER, SHGSI.SHGSI_LARGEICON)).Value;
        public static Icon RecyclerFullLarge => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_RECYCLERFULL, SHGSI.SHGSI_LARGEICON)).Value;
        public static Icon MediaCdAudioLarge => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_MEDIACDAUDIO, SHGSI.SHGSI_LARGEICON)).Value;
        public static Icon LockLarge => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_LOCK, SHGSI.SHGSI_LARGEICON)).Value;
        public static Icon AutoListLarge => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_AUTOLIST, SHGSI.SHGSI_LARGEICON)).Value;
        public static Icon PrinterNetLarge => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_PRINTERNET, SHGSI.SHGSI_LARGEICON)).Value;
        public static Icon ServerShareLarge => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_SERVERSHARE, SHGSI.SHGSI_LARGEICON)).Value;
        public static Icon PrinterFaxLarge => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_PRINTERFAX, SHGSI.SHGSI_LARGEICON)).Value;
        public static Icon PrinterFaxNetLarge => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_PRINTERFAXNET, SHGSI.SHGSI_LARGEICON)).Value;
        public static Icon PrinterFileLarge => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_PRINTERFILE, SHGSI.SHGSI_LARGEICON)).Value;
        public static Icon StackLarge => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_STACK, SHGSI.SHGSI_LARGEICON)).Value;
        public static Icon MediaSVCdLarge => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_MEDIASVCD, SHGSI.SHGSI_LARGEICON)).Value;
        public static Icon StuffedFolderLarge => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_STUFFEDFOLDER, SHGSI.SHGSI_LARGEICON)).Value;
        public static Icon DriveUnknownLarge => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_DRIVEUNKNOWN, SHGSI.SHGSI_LARGEICON)).Value;
        public static Icon DriveDvdLarge => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_DRIVEDVD, SHGSI.SHGSI_LARGEICON)).Value;
        public static Icon MediaDvdLarge => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_MEDIADVD, SHGSI.SHGSI_LARGEICON)).Value;
        public static Icon MediaDvdRamLarge => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_MEDIADVDRAM, SHGSI.SHGSI_LARGEICON)).Value;
        public static Icon MediaDvdRWLarge => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_MEDIADVDRW, SHGSI.SHGSI_LARGEICON)).Value;
        public static Icon MediaDvdRLarge => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_MEDIADVDR, SHGSI.SHGSI_LARGEICON)).Value;
        public static Icon MediaDvdRomLarge => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_MEDIADVDROM, SHGSI.SHGSI_LARGEICON)).Value;
        public static Icon MediaCdAudioPlusLarge => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_MEDIACDAUDIOPLUS, SHGSI.SHGSI_LARGEICON)).Value;
        public static Icon MediaCdRWLarge => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_MEDIACDRW, SHGSI.SHGSI_LARGEICON)).Value;
        public static Icon MediaCdRLarge => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_MEDIACDR, SHGSI.SHGSI_LARGEICON)).Value;
        public static Icon MediaCdBurnLarge => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_MEDIACDBURN, SHGSI.SHGSI_LARGEICON)).Value;
        public static Icon MediaBlankCdLarge => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_MEDIABLANKCD, SHGSI.SHGSI_LARGEICON)).Value;
        public static Icon MediaCdRomLarge => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_MEDIACDROM, SHGSI.SHGSI_LARGEICON)).Value;
        public static Icon AudioFilesLarge => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_AUDIOFILES, SHGSI.SHGSI_LARGEICON)).Value;
        public static Icon ImageFilesLarge => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_IMAGEFILES, SHGSI.SHGSI_LARGEICON)).Value;
        public static Icon VideoFilesLarge => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_VIDEOFILES, SHGSI.SHGSI_LARGEICON)).Value;
        public static Icon MixedFilesLarge => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_MIXEDFILES, SHGSI.SHGSI_LARGEICON)).Value;
        public static Icon FolderBackLarge => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_FOLDERBACK, SHGSI.SHGSI_LARGEICON)).Value;
        public static Icon FolderFrontLarge => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_FOLDERFRONT, SHGSI.SHGSI_LARGEICON)).Value;
        public static Icon ShieldLarge => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_SHIELD, SHGSI.SHGSI_LARGEICON)).Value;
        public static Icon WarningLarge => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_WARNING, SHGSI.SHGSI_LARGEICON)).Value;
        public static Icon InfoLarge => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_INFO, SHGSI.SHGSI_LARGEICON)).Value;
        public static Icon ErrorLarge => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_ERROR, SHGSI.SHGSI_LARGEICON)).Value;
        public static Icon KeyLarge => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_KEY, SHGSI.SHGSI_LARGEICON)).Value;
        public static Icon SoftwareLarge => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_SOFTWARE, SHGSI.SHGSI_LARGEICON)).Value;
        public static Icon RenameLarge => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_RENAME, SHGSI.SHGSI_LARGEICON)).Value;
        public static Icon DeleteLarge => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_DELETE, SHGSI.SHGSI_LARGEICON)).Value;
        public static Icon MediaAudioDvdLarge => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_MEDIAAUDIODVD, SHGSI.SHGSI_LARGEICON)).Value;
        public static Icon MediaMovieDvdLarge => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_MEDIAMOVIEDVD, SHGSI.SHGSI_LARGEICON)).Value;
        public static Icon MediaEnhancedCdLarge => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_MEDIAENHANCEDCD, SHGSI.SHGSI_LARGEICON)).Value;
        public static Icon MediaEnhancedDvdLarge => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_MEDIAENHANCEDDVD, SHGSI.SHGSI_LARGEICON)).Value;
        public static Icon MediaHDDvdLarge => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_MEDIAHDDVD, SHGSI.SHGSI_LARGEICON)).Value;
        public static Icon MediaBlurayLarge => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_MEDIABLURAY, SHGSI.SHGSI_LARGEICON)).Value;
        public static Icon MediaVCdLarge => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_MEDIAVCD, SHGSI.SHGSI_LARGEICON)).Value;
        public static Icon MediaDvdPlusRLarge => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_MEDIADVDPLUSR, SHGSI.SHGSI_LARGEICON)).Value;
        public static Icon MediaDvdPlusRWLarge => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_MEDIADVDPLUSRW, SHGSI.SHGSI_LARGEICON)).Value;
        public static Icon DesktopPcLarge => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_DESKTOPPC, SHGSI.SHGSI_LARGEICON)).Value;
        public static Icon MobilePcLarge => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_MOBILEPC, SHGSI.SHGSI_LARGEICON)).Value;
        public static Icon UsersLarge => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_USERS, SHGSI.SHGSI_LARGEICON)).Value;
        public static Icon MediaSmartMediLarge => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_MEDIASMARTMEDIA, SHGSI.SHGSI_LARGEICON)).Value;
        public static Icon MediaCompactFlashLarge => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_MEDIACOMPACTFLASH, SHGSI.SHGSI_LARGEICON)).Value;
        public static Icon DeviceCellPhoneLarge => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_DEVICECELLPHONE, SHGSI.SHGSI_LARGEICON)).Value;
        public static Icon DeviceCameraLarge => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_DEVICECAMERA, SHGSI.SHGSI_LARGEICON)).Value;
        public static Icon DeviceVideoCameraLarge => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_DEVICEVIDEOCAMERA, SHGSI.SHGSI_LARGEICON)).Value;
        public static Icon DeviceAudioPlayerLarge => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_DEVICEAUDIOPLAYER, SHGSI.SHGSI_LARGEICON)).Value;
        public static Icon NetworkConnectLarge => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_NETWORKCONNECT, SHGSI.SHGSI_LARGEICON)).Value;
        public static Icon InternetLarge => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_INTERNET, SHGSI.SHGSI_LARGEICON)).Value;
        public static Icon ZipFileLarge => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_ZIPFILE, SHGSI.SHGSI_LARGEICON)).Value;
        public static Icon SettingsLarge => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_SETTINGS, SHGSI.SHGSI_LARGEICON)).Value;
        public static Icon DriveHDDvdLarge => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_DRIVEHDDVD, SHGSI.SHGSI_LARGEICON)).Value;
        public static Icon DriveBdLarge => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_DRIVEBD, SHGSI.SHGSI_LARGEICON)).Value;
        public static Icon MediaHDDvdRomLarge => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_MEDIAHDDVDROM, SHGSI.SHGSI_LARGEICON)).Value;
        public static Icon MediaHDDvdRLarge => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_MEDIAHDDVDR, SHGSI.SHGSI_LARGEICON)).Value;
        public static Icon MediaHDDvdRamLarge => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_MEDIAHDDVDRAM, SHGSI.SHGSI_LARGEICON)).Value;
        public static Icon MediaBdRomLarge => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_MEDIABDROM, SHGSI.SHGSI_LARGEICON)).Value;
        public static Icon MediaBdRLarge => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_MEDIABDR, SHGSI.SHGSI_LARGEICON)).Value;
        public static Icon MediaBdRWLarge => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_MEDIABDRE, SHGSI.SHGSI_LARGEICON)).Value;
        public static Icon ClusteredDriveLarge => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_CLUSTEREDDRIVE, SHGSI.SHGSI_LARGEICON)).Value;

        public static Icon DocNoAssocSmall => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_DOCNOASSOC, SHGSI.SHGSI_SMALLICON)).Value;
        public static Icon DocAssocSmall => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_DOCASSOC, SHGSI.SHGSI_SMALLICON)).Value;
        public static Icon ApplicationSmall => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_APPLICATION, SHGSI.SHGSI_SMALLICON)).Value;
        public static Icon FolderSmall => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_FOLDER, SHGSI.SHGSI_SMALLICON)).Value;
        public static Icon FolderOpenSmall => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_FOLDEROPEN, SHGSI.SHGSI_SMALLICON)).Value;
        public static Icon Drive525Small => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_DRIVE525, SHGSI.SHGSI_SMALLICON)).Value;
        public static Icon Drive35Small => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_DRIVE35, SHGSI.SHGSI_SMALLICON)).Value;
        public static Icon DriveRemoveSmall => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_DRIVEREMOVE, SHGSI.SHGSI_SMALLICON)).Value;
        public static Icon DriveFixedSmall => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_DRIVEFIXED, SHGSI.SHGSI_SMALLICON)).Value;
        public static Icon DriveNetSmall => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_DRIVENET, SHGSI.SHGSI_SMALLICON)).Value;
        public static Icon DriveNetDisabledSmall => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_DRIVENETDISABLED, SHGSI.SHGSI_SMALLICON)).Value;
        public static Icon DriveCdSmall => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_DRIVECD, SHGSI.SHGSI_SMALLICON)).Value;
        public static Icon DriveRamSmall => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_DRIVERAM, SHGSI.SHGSI_SMALLICON)).Value;
        public static Icon WorldSmall => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_WORLD, SHGSI.SHGSI_SMALLICON)).Value;
        public static Icon ServerSmall => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_SERVER, SHGSI.SHGSI_SMALLICON)).Value;
        public static Icon PrinterSmall => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_PRINTER, SHGSI.SHGSI_SMALLICON)).Value;
        public static Icon MyNetworkSmall => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_MYNETWORK, SHGSI.SHGSI_SMALLICON)).Value;
        public static Icon FindSmall => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_FIND, SHGSI.SHGSI_SMALLICON)).Value;
        public static Icon HelpSmall => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_HELP, SHGSI.SHGSI_SMALLICON)).Value;
        public static Icon ShareSmall => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_SHARE, SHGSI.SHGSI_SMALLICON)).Value;
        public static Icon LinkSmall => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_LINK, SHGSI.SHGSI_SMALLICON)).Value;
        public static Icon SlowFileSmall => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_SLOWFILE, SHGSI.SHGSI_SMALLICON)).Value;
        public static Icon RecyclerSmall => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_RECYCLER, SHGSI.SHGSI_SMALLICON)).Value;
        public static Icon RecyclerFullSmall => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_RECYCLERFULL, SHGSI.SHGSI_SMALLICON)).Value;
        public static Icon MediaCdAudioSmall => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_MEDIACDAUDIO, SHGSI.SHGSI_SMALLICON)).Value;
        public static Icon LockSmall => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_LOCK, SHGSI.SHGSI_SMALLICON)).Value;
        public static Icon AutoListSmall => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_AUTOLIST, SHGSI.SHGSI_SMALLICON)).Value;
        public static Icon PrinterNetSmall => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_PRINTERNET, SHGSI.SHGSI_SMALLICON)).Value;
        public static Icon ServerShareSmall => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_SERVERSHARE, SHGSI.SHGSI_SMALLICON)).Value;
        public static Icon PrinterFaxSmall => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_PRINTERFAX, SHGSI.SHGSI_SMALLICON)).Value;
        public static Icon PrinterFaxNetSmall => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_PRINTERFAXNET, SHGSI.SHGSI_SMALLICON)).Value;
        public static Icon PrinterFileSmall => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_PRINTERFILE, SHGSI.SHGSI_SMALLICON)).Value;
        public static Icon StackSmall => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_STACK, SHGSI.SHGSI_SMALLICON)).Value;
        public static Icon MediaSVCdSmall => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_MEDIASVCD, SHGSI.SHGSI_SMALLICON)).Value;
        public static Icon StuffedFolderSmall => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_STUFFEDFOLDER, SHGSI.SHGSI_SMALLICON)).Value;
        public static Icon DriveUnknownSmall => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_DRIVEUNKNOWN, SHGSI.SHGSI_SMALLICON)).Value;
        public static Icon DriveDvdSmall => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_DRIVEDVD, SHGSI.SHGSI_SMALLICON)).Value;
        public static Icon MediaDvdSmall => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_MEDIADVD, SHGSI.SHGSI_SMALLICON)).Value;
        public static Icon MediaDvdRamSmall => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_MEDIADVDRAM, SHGSI.SHGSI_SMALLICON)).Value;
        public static Icon MediaDvdRWSmall => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_MEDIADVDRW, SHGSI.SHGSI_SMALLICON)).Value;
        public static Icon MediaDvdRSmall => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_MEDIADVDR, SHGSI.SHGSI_SMALLICON)).Value;
        public static Icon MediaDvdRomSmall => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_MEDIADVDROM, SHGSI.SHGSI_SMALLICON)).Value;
        public static Icon MediaCdAudioPlusSmall => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_MEDIACDAUDIOPLUS, SHGSI.SHGSI_SMALLICON)).Value;
        public static Icon MediaCdRWSmall => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_MEDIACDRW, SHGSI.SHGSI_SMALLICON)).Value;
        public static Icon MediaCdRSmall => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_MEDIACDR, SHGSI.SHGSI_SMALLICON)).Value;
        public static Icon MediaCdBurnSmall => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_MEDIACDBURN, SHGSI.SHGSI_SMALLICON)).Value;
        public static Icon MediaBlankCdSmall => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_MEDIABLANKCD, SHGSI.SHGSI_SMALLICON)).Value;
        public static Icon MediaCdRomSmall => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_MEDIACDROM, SHGSI.SHGSI_SMALLICON)).Value;
        public static Icon AudioFilesSmall => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_AUDIOFILES, SHGSI.SHGSI_SMALLICON)).Value;
        public static Icon ImageFilesSmall => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_IMAGEFILES, SHGSI.SHGSI_SMALLICON)).Value;
        public static Icon VideoFilesSmall => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_VIDEOFILES, SHGSI.SHGSI_SMALLICON)).Value;
        public static Icon MixedFilesSmall => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_MIXEDFILES, SHGSI.SHGSI_SMALLICON)).Value;
        public static Icon FolderBackSmall => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_FOLDERBACK, SHGSI.SHGSI_SMALLICON)).Value;
        public static Icon FolderFrontSmall => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_FOLDERFRONT, SHGSI.SHGSI_SMALLICON)).Value;
        public static Icon ShieldSmall => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_SHIELD, SHGSI.SHGSI_SMALLICON)).Value;
        public static Icon WarningSmall => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_WARNING, SHGSI.SHGSI_SMALLICON)).Value;
        public static Icon InfoSmall => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_INFO, SHGSI.SHGSI_SMALLICON)).Value;
        public static Icon ErrorSmall => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_ERROR, SHGSI.SHGSI_SMALLICON)).Value;
        public static Icon KeySmall => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_KEY, SHGSI.SHGSI_SMALLICON)).Value;
        public static Icon SoftwareSmall => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_SOFTWARE, SHGSI.SHGSI_SMALLICON)).Value;
        public static Icon RenameSmall => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_RENAME, SHGSI.SHGSI_SMALLICON)).Value;
        public static Icon DeleteSmall => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_DELETE, SHGSI.SHGSI_SMALLICON)).Value;
        public static Icon MediaAudioDvdSmall => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_MEDIAAUDIODVD, SHGSI.SHGSI_SMALLICON)).Value;
        public static Icon MediaMovieDvdSmall => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_MEDIAMOVIEDVD, SHGSI.SHGSI_SMALLICON)).Value;
        public static Icon MediaEnhancedCdSmall => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_MEDIAENHANCEDCD, SHGSI.SHGSI_SMALLICON)).Value;
        public static Icon MediaEnhancedDvdSmall => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_MEDIAENHANCEDDVD, SHGSI.SHGSI_SMALLICON)).Value;
        public static Icon MediaHDDvdSmall => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_MEDIAHDDVD, SHGSI.SHGSI_SMALLICON)).Value;
        public static Icon MediaBluraySmall => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_MEDIABLURAY, SHGSI.SHGSI_SMALLICON)).Value;
        public static Icon MediaVCdSmall => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_MEDIAVCD, SHGSI.SHGSI_SMALLICON)).Value;
        public static Icon MediaDvdPlusRSmall => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_MEDIADVDPLUSR, SHGSI.SHGSI_SMALLICON)).Value;
        public static Icon MediaDvdPlusRWSmall => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_MEDIADVDPLUSRW, SHGSI.SHGSI_SMALLICON)).Value;
        public static Icon DesktopPcSmall => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_DESKTOPPC, SHGSI.SHGSI_SMALLICON)).Value;
        public static Icon MobilePcSmall => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_MOBILEPC, SHGSI.SHGSI_SMALLICON)).Value;
        public static Icon UsersSmall => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_USERS, SHGSI.SHGSI_SMALLICON)).Value;
        public static Icon MediaSmartMedisSmall => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_MEDIASMARTMEDIA, SHGSI.SHGSI_SMALLICON)).Value;
        public static Icon MediaCompactFlashSmall => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_MEDIACOMPACTFLASH, SHGSI.SHGSI_SMALLICON)).Value;
        public static Icon DeviceCellPhoneSmall => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_DEVICECELLPHONE, SHGSI.SHGSI_SMALLICON)).Value;
        public static Icon DeviceCameraSmall => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_DEVICECAMERA, SHGSI.SHGSI_SMALLICON)).Value;
        public static Icon DeviceVideoCameraSmall => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_DEVICEVIDEOCAMERA, SHGSI.SHGSI_SMALLICON)).Value;
        public static Icon DeviceAudioPlayerSmall => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_DEVICEAUDIOPLAYER, SHGSI.SHGSI_SMALLICON)).Value;
        public static Icon NetworkConnectSmall => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_NETWORKCONNECT, SHGSI.SHGSI_SMALLICON)).Value;
        public static Icon InternetSmall => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_INTERNET, SHGSI.SHGSI_SMALLICON)).Value;
        public static Icon ZipFileSmall => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_ZIPFILE, SHGSI.SHGSI_SMALLICON)).Value;
        public static Icon SettingsSmall => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_SETTINGS, SHGSI.SHGSI_SMALLICON)).Value;
        public static Icon DriveHDDvdSmall => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_DRIVEHDDVD, SHGSI.SHGSI_SMALLICON)).Value;
        public static Icon DriveBdSmall => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_DRIVEBD, SHGSI.SHGSI_SMALLICON)).Value;
        public static Icon MediaHDDvdRomSmall => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_MEDIAHDDVDROM, SHGSI.SHGSI_SMALLICON)).Value;
        public static Icon MediaHDDvdRSmall => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_MEDIAHDDVDR, SHGSI.SHGSI_SMALLICON)).Value;
        public static Icon MediaHDDvdRamSmall => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_MEDIAHDDVDRAM, SHGSI.SHGSI_SMALLICON)).Value;
        public static Icon MediaBdRomSmall => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_MEDIABDROM, SHGSI.SHGSI_SMALLICON)).Value;
        public static Icon MediaBdRSmall => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_MEDIABDR, SHGSI.SHGSI_SMALLICON)).Value;
        public static Icon MediaBdRWSmall => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_MEDIABDRE, SHGSI.SHGSI_SMALLICON)).Value;
        public static Icon ClusteredDriveSmall => new Lazy<Icon>(() => Load(SHSTOCKICONID.SIID_CLUSTEREDDRIVE, SHGSI.SHGSI_SMALLICON)).Value;

        private static Icon Load(SHSTOCKICONID stockIconId, SHGSI gsi)
        {
            SHSTOCKICONINFO iconInfo = new SHSTOCKICONINFO
            {
                cbSize = SHSTOCKICONINFO_SIZE,
            };

            SHGetStockIconInfo(stockIconId, SHGSI.SHGSI_ICON | gsi, ref iconInfo);

            return Icon.FromHandle(iconInfo.hIcon);
        }


        private const int MAX_PATH = 260;


        [Flags]
        public enum SHGSI : uint
        {
            SHGSI_ICONLOCATION = 0,
            SHGSI_ICON = 0x000000100,
            SHGSI_SYSICONINDEX = 0x000004000,
            SHGSI_LINKOVERLAY = 0x000008000,
            SHGSI_SELECTED = 0x000010000,
            SHGSI_LARGEICON = 0x000000000,
            SHGSI_SMALLICON = 0x000000001,
            SHGSI_SHELLICONSIZE = 0x000000004
        }

        [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct SHSTOCKICONINFO
        {
            public UInt32 cbSize;
            public IntPtr hIcon;
            public Int32 iSysIconIndex;
            public Int32 iIcon;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_PATH)]
            public string szPath;
        }

        private static readonly uint SHSTOCKICONINFO_SIZE = (uint)Marshal.SizeOf(typeof(SHSTOCKICONINFO));

        [DllImport("Shell32.dll", SetLastError = false)]
        private static extern Int32 SHGetStockIconInfo(SHSTOCKICONID siid, SHGSI uFlags, ref SHSTOCKICONINFO psii);

        private enum SHSTOCKICONID : uint
        {
            SIID_DOCNOASSOC = 0,
            SIID_DOCASSOC = 1,
            SIID_APPLICATION = 2,
            SIID_FOLDER = 3,
            SIID_FOLDEROPEN = 4,
            SIID_DRIVE525 = 5,
            SIID_DRIVE35 = 6,
            SIID_DRIVEREMOVE = 7,
            SIID_DRIVEFIXED = 8,
            SIID_DRIVENET = 9,
            SIID_DRIVENETDISABLED = 10,
            SIID_DRIVECD = 11,
            SIID_DRIVERAM = 12,
            SIID_WORLD = 13,
            SIID_SERVER = 15,
            SIID_PRINTER = 16,
            SIID_MYNETWORK = 17,
            SIID_FIND = 22,
            SIID_HELP = 23,
            SIID_SHARE = 28,
            SIID_LINK = 29,
            SIID_SLOWFILE = 30,
            SIID_RECYCLER = 31,
            SIID_RECYCLERFULL = 32,
            SIID_MEDIACDAUDIO = 40,
            SIID_LOCK = 47,
            SIID_AUTOLIST = 49,
            SIID_PRINTERNET = 50,
            SIID_SERVERSHARE = 51,
            SIID_PRINTERFAX = 52,
            SIID_PRINTERFAXNET = 53,
            SIID_PRINTERFILE = 54,
            SIID_STACK = 55,
            SIID_MEDIASVCD = 56,
            SIID_STUFFEDFOLDER = 57,
            SIID_DRIVEUNKNOWN = 58,
            SIID_DRIVEDVD = 59,
            SIID_MEDIADVD = 60,
            SIID_MEDIADVDRAM = 61,
            SIID_MEDIADVDRW = 62,
            SIID_MEDIADVDR = 63,
            SIID_MEDIADVDROM = 64,
            SIID_MEDIACDAUDIOPLUS = 65,
            SIID_MEDIACDRW = 66,
            SIID_MEDIACDR = 67,
            SIID_MEDIACDBURN = 68,
            SIID_MEDIABLANKCD = 69,
            SIID_MEDIACDROM = 70,
            SIID_AUDIOFILES = 71,
            SIID_IMAGEFILES = 72,
            SIID_VIDEOFILES = 73,
            SIID_MIXEDFILES = 74,
            SIID_FOLDERBACK = 75,
            SIID_FOLDERFRONT = 76,
            SIID_SHIELD = 77,
            SIID_WARNING = 78,
            SIID_INFO = 79,
            SIID_ERROR = 80,
            SIID_KEY = 81,
            SIID_SOFTWARE = 82,
            SIID_RENAME = 83,
            SIID_DELETE = 84,
            SIID_MEDIAAUDIODVD = 85,
            SIID_MEDIAMOVIEDVD = 86,
            SIID_MEDIAENHANCEDCD = 87,
            SIID_MEDIAENHANCEDDVD = 88,
            SIID_MEDIAHDDVD = 89,
            SIID_MEDIABLURAY = 90,
            SIID_MEDIAVCD = 91,
            SIID_MEDIADVDPLUSR = 92,
            SIID_MEDIADVDPLUSRW = 93,
            SIID_DESKTOPPC = 94,
            SIID_MOBILEPC = 95,
            SIID_USERS = 96,
            SIID_MEDIASMARTMEDIA = 97,
            SIID_MEDIACOMPACTFLASH = 98,
            SIID_DEVICECELLPHONE = 99,
            SIID_DEVICECAMERA = 100,
            SIID_DEVICEVIDEOCAMERA = 101,
            SIID_DEVICEAUDIOPLAYER = 102,
            SIID_NETWORKCONNECT = 103,
            SIID_INTERNET = 104,
            SIID_ZIPFILE = 105,
            SIID_SETTINGS = 106,
            SIID_DRIVEHDDVD = 132,
            SIID_DRIVEBD = 133,
            SIID_MEDIAHDDVDROM = 134,
            SIID_MEDIAHDDVDR = 135,
            SIID_MEDIAHDDVDRAM = 136,
            SIID_MEDIABDROM = 137,
            SIID_MEDIABDR = 138,
            SIID_MEDIABDRE = 139,
            SIID_CLUSTEREDDRIVE = 140,
            SIID_MAX_ICONS = 175
        }
    }
}
