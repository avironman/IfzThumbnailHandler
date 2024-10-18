using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using static System.Resources.ResXFileRef;

namespace OMRON_IFZ_Viewer
{
    /// <summary>
    /// FZ_ImageProcess DLL Wrapper
    /// </summary>
    internal static class FZ_ImageProcess
    {

        /// <summary>
        /// DLL Name
        /// </summary>
        const string FZ_ImageProcessDllName = "FZ-ImageProcess.dll";

        /// <summary>
        /// Preload the FZ-ImageProcess DLL
        /// </summary>
        static FZ_ImageProcess()
        {
            // Check the size of the pointer
            string folder = IntPtr.Size == 8 ? "x64" : "x86";
            // Build the full library file name
            string libraryFile1 = Path.Combine(Path.GetDirectoryName(typeof(FZ_ImageProcess).Assembly.Location), folder, "FZ-TransCoordinate.dll");
            string libraryFile2 = Path.Combine(Path.GetDirectoryName(typeof(FZ_ImageProcess).Assembly.Location), folder, "FZ-Halcon.dll");
            string libraryFile3 = Path.Combine(Path.GetDirectoryName(typeof(FZ_ImageProcess).Assembly.Location), folder, "FZ-Parallel.dll");
            string libraryFile4 = Path.Combine(Path.GetDirectoryName(typeof(FZ_ImageProcess).Assembly.Location), folder, FZ_ImageProcessDllName);
            // Load the library

            LoadLibrary(libraryFile1);
            LoadLibrary(libraryFile2);
            LoadLibrary(libraryFile3);
            var res = LoadLibrary(libraryFile4);
             
            if (res == IntPtr.Zero)
            {

                Console.WriteLine(libraryFile1 + "-> Erreur " + Marshal.GetLastWin32Error().ToString());
                throw new InvalidOperationException("Failed to load the library.");
            }
            
            
        }

        [DllImport("kernel32.dll", CharSet=CharSet.Auto, SetLastError = false)]
        public static extern IntPtr LoadLibrary(string dllToLoad);

        //[DllImport("Kernel32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, SetLastError = false)]
        //private static extern IntPtr LoadLibrary([MarshalAs(UnmanagedType.LPStr)] string lpFileName);


        /// <summary>
        /// Function from the FZ-ImageProcess DLL
        /// </summary>
        [DllImport(FZ_ImageProcessDllName, CharSet = CharSet.None, ExactSpelling = false)]
        public static extern unsafe int BayerToRGBImage2(byte[] ip, int width, int height, int x1, int y1, int x2, int y2, int arrayType, byte* p);

        [DllImport(FZ_ImageProcessDllName, CharSet = CharSet.None, ExactSpelling = false)]
        public static extern unsafe int ImageToBMP2(byte* ip, int format, int width, int height, byte[] op);

        [DllImport(FZ_ImageProcessDllName, CharSet = CharSet.None, ExactSpelling = false)]
        public static extern int ImageToBMPSize2(int format, int width, int height);

        [DllImport(FZ_ImageProcessDllName, CharSet = CharSet.None, ExactSpelling = false)]
        public static extern unsafe int BayerToRGBImage(byte[] ip, int width, int height, int x1, int y1, int x2, int y2, int arrayType, int maskSize, byte* p);

        [DllImport(FZ_ImageProcessDllName, CharSet = CharSet.None, ExactSpelling = false)]
        public static extern unsafe int YUV422ToImage(byte[] ip, int width, int height, int x1, int y1, int x2, int y2, byte* p);

        [DllImport(FZ_ImageProcessDllName, CharSet = CharSet.None, ExactSpelling = false)]
        public static extern unsafe int MakeGrayImage(byte[] ip, int width, int height, int x1, int y1, int x2, int y2, int rate, byte* p);

        [DllImport(FZ_ImageProcessDllName, CharSet = CharSet.None, ExactSpelling = false)]
        public static extern unsafe int RGB24ToImage(byte[] ip, int width, int height, int x1, int y1, int x2, int y2, byte* p);

    }
}

