using System;
using System.Runtime.InteropServices;

namespace MyIE
{
    public class IEP
    {

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct INTERNET_CACHE_ENTRY_INFO
        {
            public int dwStructSize;
            public IntPtr lpszSourceUrlName;
            public IntPtr lpszLocalFileName;
            public int CacheEntryType;
            public int dwUseCount;
            public int dwHitRate;
            public int dwSizeLow;
            public int dwSizeHigh;
            public System.Runtime.InteropServices.ComTypes.FILETIME LastModifiedTime;
            public System.Runtime.InteropServices.ComTypes.FILETIME ExpireTime;
            public System.Runtime.InteropServices.ComTypes.FILETIME LastAccessTime;
            public System.Runtime.InteropServices.ComTypes.FILETIME LastSyncTime;
            public IntPtr lpHeaderInfo;
            public int dwHeaderInfoSize;
            public IntPtr lpszFileExtension;
            public int dwExemptDelta;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct SYSTEMTIME
        {
            public short wYear;
            public short wMonth;
            public short wDayOfWeek;
            public short wDay;
            public short wHour;
            public short wMinute;
            public short wSecond;
            public short wMilliseconds;
        }


        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int FileTimeToSystemTime(
         IntPtr lpFileTime,
         IntPtr lpSystemTime);

        [DllImport("wininet", SetLastError = true, CharSet = CharSet.Auto, EntryPoint = "FindFirstUrlCacheGr oup", CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr FindFirstUrlCacheGroup(
         int dwFlags,
         int dwFilter,
         IntPtr lpSearchCondition,
         int dwSearchCondition,
         ref long lpGroupId,
         IntPtr lpReserved);

        [DllImport("wininet", SetLastError = true, CharSet = CharSet.Auto, EntryPoint = "FindNextUrlCacheGroup", CallingConvention = CallingConvention.StdCall)]
        public static extern bool FindNextUrlCacheGroup(
         IntPtr hFind,
         ref long lpGroupId,
         IntPtr lpReserved);

        [DllImport("wininet", SetLastError = true, CharSet = CharSet.Auto, EntryPoint = "De leteUrlCacheGroup", CallingConvention = CallingConvention.StdCall)]
        public static extern bool DeleteUrlCacheGroup(
         long GroupId,
         int dwFlags,
         IntPtr lpReserved);


        [DllImport("wininet.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr FindFirstUrlCacheEntry([MarshalAs(UnmanagedType.LPTStr)] string UrlSearchPattern, IntPtr lpFirstCacheEntryInfo, ref int lpdwFirstCacheEntryInfoBufferSize);

        [DllImport("wininet.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool FindNextUrlCacheEntry(
          IntPtr hEnumHandle,
          IntPtr lpNextCacheEntryInfo,
          ref int lpdwNextCacheEntryInfoBufferSize);

        [DllImport("wininet.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr FindFirstUrlCacheEntryEx(
            [MarshalAs(UnmanagedType.LPTStr)] string UrlSearchPattern,
            int dwFlags,
            int dwFilter,
            Int64 GroupId,
            IntPtr lpFirstCacheEntryInfo,
            ref int dwFirstCacheEntryInfoBufferSize,
            IntPtr lpReserved,
            IntPtr cbReserved2,
            IntPtr lpReserved3
        );

        [DllImport("wininet.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool FindNextUrlCacheEntryEx(
            IntPtr hEnumHandle,
            IntPtr lpFirstCacheEntryInfo,
            ref int dwFirstCacheEntryInfoBufferSize,
            IntPtr lpReserved,
            IntPtr cbReserved2,
            IntPtr lpReserved3
        );

        [DllImport("wininet.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool FindCloseUrlCache(IntPtr hEnumHandle);



        [DllImport("wininet.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool GetUrlCacheEntryInfo(
         [MarshalAs(UnmanagedType.LPTStr)] string lpszUrlName,
         IntPtr lpCacheEntryInfo,
         ref int lpdwCacheEntryInfoBufferSize
         );


        [DllImport("wininet",
          SetLastError = true,
          CharSet = CharSet.Auto,
          EntryPoint = "DeleteUrlCacheEntryA",
          CallingConvention = CallingConvention.StdCall)]
        public static extern bool DeleteUrlCacheEntry(
         IntPtr lpszUrlName);

        public enum CacheGroup : uint
        {
            /// 
            /// Retrieves the flags, type, and disk quota attributes of the cache group. This is used by the GetUrlCacheGroupAttribute function. 
            /// 
            CACHEGROUP_ATTRIBUTE_BASIC = 0x00000001,
            /// 
            /// Sets or retrieves the flags associated with the cache group. This is used by the GetUrlCacheGroupAttribute and SetUrlCacheGroupAttribute functions. 
            /// 
            CACHEGROUP_ATTRIBUTE_FLAG = 0x00000002,

            /// 
            /// Retrieves all the attributes of the cache group. This is used by the GetUrlCacheGroupAttribute function. 
            /// 
            CACHEGROUP_ATTRIBUTE_GET_ALL = 0xffffffff,

            /// 
            /// Sets or retrieves the group name of the cache group. This is used by the GetUrlCacheGroupAttribute and SetUrlCacheGroupAttribute functions. 
            /// 
            CACHEGROUP_ATTRIBUTE_GROUPNAME = 0x000000010,

            /// 
            /// Sets or retrieves the disk quota associated with the cache group. This is used by the GetUrlCacheGroupAttribute and SetUrlCacheGroupAttribute functions. 
            /// 
            CACHEGROUP_ATTRIBUTE_QUOTA = 0x00000008,

            /// 
            /// Sets or retrieves the group owner storage associated with the cache group. This is used by the GetUrlCacheGroupAttribute and SetUrlCacheGroupAttribute functions. 
            /// 
            CACHEGROUP_ATTRIBUTE_STORAGE = 0x00000020,

            /// 
            /// Sets or retrieves the cache group type. This is used by the GetUrlCacheGroupAttribute and SetUrlCacheGroupAttribute functions. 
            /// 
            CACHEGROUP_ATTRIBUTE_TYPE = 0x00000004,

            /// 
            /// Indicates that all the cache entries associated with the cache group should be deleted, unless the entry belongs to another cache group. 
            /// 
            CACHEGROUP_FLAG_FLUSHURL_ONDELETE = 0x00000002,

            /// 
            /// Indicates that the function should only create a unique GROUPID for the cache group and not create the actual group. 
            /// 
            CACHEGROUP_FLAG_GIDONLY = 0x00000004,

            /// 
            /// Indicates that the cache group cannot be purged. 
            /// 
            CACHEGROUP_FLAG_NONPURGEABLE = 0x00000001,

            /// 
            /// Sets the type, disk quota, group name, and owner storage attributes of the cache group. This is used by the SetUrlCacheGroupAttribute function. 
            /// 
            CACHEGROUP_READWRITE_MASK = 0x0000003c,

            /// 
            /// Indicates that all of the cache groups in the user's system should be enumerated. 
            /// 
            CACHEGROUP_SEARCH_ALL = 0x00000000,

            /// 
            /// Not currently implemented. 
            /// 
            CACHEGROUP_SEARCH_BYURL = 0x00000001,

            /// 
            /// Indicates that the cache group type is invalid. 
            /// 
            CACHEGROUP_TYPE_INVALID = 0x00000001,

        }

        [Flags]
        public enum CacheError : int
        {
            ERROR_CACHE_FIND_FAIL = 0,
            ERROR_CACHE_FIND_SUCCESS = 1,
            ERROR_FILE_NOT_FOUND = 2,
            ERROR_ACCESS_DENIED = 5,
            ERROR_INSUFFICIENT_BUFFER = 122,
            ERROR_NO_MORE_ITEMS = 259,
        }

        [Flags]
        public enum CacheEntry : int
        {
            NORMAL_CACHE_ENTRY = 0x1,
            EDITED_CACHE_ENTRY = 0x8,
            TRACK_OFFLINE_CACHE_ENTRY = 0x10,
            TRACK_ONLINE_CACHE_ENTRY = 0x20,
            STICKY_CACHE_ENTRY = 0x40,
            SPARSE_CACHE_ENTRY = 0x10000,
            COOKIE_CACHE_ENTRY = 0x100000,
            URLHISTORY_CACHE_ENTRY = 0x200000,
            URLCACHE_FIND_DEFAULT_FILTER = NORMAL_CACHE_ENTRY
                | EDITED_CACHE_ENTRY
                | TRACK_OFFLINE_CACHE_ENTRY
                | TRACK_ONLINE_CACHE_ENTRY
                | STICKY_CACHE_ENTRY
                | SPARSE_CACHE_ENTRY
                | COOKIE_CACHE_ENTRY
                | URLHISTORY_CACHE_ENTRY,

        }

        public enum CacheConst : long
        {
            /// 
            /// Length of the group owner storage array. 
            /// 
            GROUP_OWNER_STORAGE_SIZE = 0x00000004,

            /// 
            /// Maximum number of characters allowed for a cache group name.
            /// 
            GROUPNAME_MAX_LENGTH = 0x00000078,

            MAX_PATH = 260,
            MAX_CACHE_ENTRY_INFO_SIZE = 4096,

            LMEM_FIXED = 0x0,
            LMEM_ZEROINIT = 0x40,
            LPTR = (LMEM_FIXED | LMEM_ZEROINIT),
        }
    }
}
