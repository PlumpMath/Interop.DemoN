/*
Copyright (c) 2013 Nathan LeRoux

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/
using System;
using System.Threading;
using System.Runtime.InteropServices;

namespace Xecuter.DemoN
{
    public enum DemoNError : uint
    {
        None = 0,
        Unknown,
        OutOfMemory,
        FileOpen,
        FileRead,
        FileWrite,
        FileFlush,
        FileSeek,
        FileFormat,
        DeviceInit,
        DeviceNotOpen,
        DeviceOpen,
        DeviceRead,
        DeviceWrite,
        DeviceIllegalValue,
        FlashUnknownID,
        FlashMaxBadBlocks,
        XSVFUnknown,
        XSVFTooLarge,
        XSVFTDOMismatch,
        XSVFMaxRetries,
        XSVFIllegalCommand,
        XSVFIllegalState,
        XSVFDataOverflow,
        XSVFRead,
        InvalidImage,
        Checksum,
        WrongFirmware,
        EnterBootloader,
        EnterFirmware
    };

    public enum DemoNFlash : uint
    {
        Xbox360 = 0,
        DemoN
    };

    static class Unmanaged
    {
        [DllImport("demon.dll", EntryPoint = "demon_create", CallingConvention = CallingConvention.Cdecl)]
        public static extern DemoNError demon_create(out IntPtr self, IntPtr printf, IntPtr delay);

        [DllImport("demon.dll", EntryPoint = "demon_destroy", CallingConvention = CallingConvention.Cdecl)]
        public static extern void demon_destroy(IntPtr self);

        [DllImport("demon.dll", EntryPoint = "demon_open", CallingConvention = CallingConvention.Cdecl)]
        public static extern DemoNError demon_open(IntPtr self);

        [DllImport("demon.dll", EntryPoint = "demon_close", CallingConvention = CallingConvention.Cdecl)]
        public static extern void demon_close(IntPtr self);

        [DllImport("demon.dll", EntryPoint = "demon_run_bootloader", CallingConvention = CallingConvention.Cdecl)]
        public static extern DemoNError demon_run_bootloader(IntPtr self);

        [DllImport("demon.dll", EntryPoint = "demon_run_firmware", CallingConvention = CallingConvention.Cdecl)]
        public static extern DemoNError demon_run_firmware(IntPtr self);

        [DllImport("demon.dll", EntryPoint = "demon_read_flash", CallingConvention = CallingConvention.Cdecl)]
        public static extern DemoNError demon_read_flash(IntPtr self, [MarshalAs(UnmanagedType.LPStr)] string filename);

        [DllImport("demon.dll", EntryPoint = "demon_program_flash", CallingConvention = CallingConvention.Cdecl)]
        public static extern DemoNError demon_program_flash(IntPtr self, [MarshalAs(UnmanagedType.LPStr)] string filename);

        [DllImport("demon.dll", EntryPoint = "demon_erase_flash", CallingConvention = CallingConvention.Cdecl)]
        public static extern DemoNError demon_erase_flash(IntPtr self);

        [DllImport("demon.dll", EntryPoint = "demon_exec_xsvf", CallingConvention = CallingConvention.Cdecl)]
        public static extern DemoNError demon_exec_xsvf(IntPtr self, [MarshalAs(UnmanagedType.LPStr)] string filename);

        [DllImport("demon.dll", EntryPoint = "demon_power_on", CallingConvention = CallingConvention.Cdecl)]
        public static extern DemoNError demon_power_on(IntPtr self);

        [DllImport("demon.dll", EntryPoint = "demon_power_off", CallingConvention = CallingConvention.Cdecl)]
        public static extern DemoNError demon_power_off(IntPtr self);

        [DllImport("demon.dll", EntryPoint = "demon_select_flash", CallingConvention = CallingConvention.Cdecl)]
        public static extern DemoNError demon_select_flash(IntPtr self, DemoNFlash flash);

        [DllImport("demon.dll", EntryPoint = "demon_update_firmware", CallingConvention = CallingConvention.Cdecl)]
        public static extern DemoNError demon_update_firmware(IntPtr self, [MarshalAs(UnmanagedType.LPStr)] string filename);

        [DllImport("demon.dll", EntryPoint = "demon_get_error_msg", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern string demon_get_error_message(DemoNError err);

        [DllImport("msvcrt.dll", EntryPoint = "vprintf", CallingConvention = CallingConvention.Cdecl)]
        public static extern int vprintf([MarshalAs(UnmanagedType.LPStr)] string format, IntPtr args);
    }
    public static class DemoN
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate int demon_printf_fn_t([MarshalAs(UnmanagedType.LPStr)] string format, IntPtr args);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate void demon_delay_fn_t(uint msecs);

        static int demon_printf_fn([MarshalAs(UnmanagedType.LPStr)] string format, IntPtr args)
        {
            return 0;
        }

        static void demon_delay_fn(uint msecs)
        {
            Thread.Sleep((int)msecs);
        }

        public static DemoNError Create(out IntPtr Handle)
        {
            return Unmanaged.demon_create(out Handle, Marshal.GetFunctionPointerForDelegate(new demon_printf_fn_t(demon_printf_fn)), Marshal.GetFunctionPointerForDelegate(new demon_delay_fn_t(demon_delay_fn)));
        }

        public static void Destroy(IntPtr Handle)
        {
            Unmanaged.demon_destroy(Handle);
        }

        public static DemoNError Open(IntPtr Handle)
        {
            return Unmanaged.demon_open(Handle);
        }

        public static void Close(IntPtr Handle)
        {
            Unmanaged.demon_close(Handle);
        }

        public static DemoNError RunBootloader(IntPtr Handle)
        {
            return Unmanaged.demon_run_bootloader(Handle);
        }

        public static DemoNError RunFirmware(IntPtr Handle)
        {
            return Unmanaged.demon_run_firmware(Handle);
        }

        public static DemoNError ReadFlash(IntPtr Handle, string Filename)
        {
            return Unmanaged.demon_read_flash(Handle, Filename);
        }

        public static DemoNError ProgramFlash(IntPtr Handle, string Filename)
        {
            return Unmanaged.demon_program_flash(Handle, Filename);
        }

        public static DemoNError EraseFlash(IntPtr Handle)
        {
            return Unmanaged.demon_erase_flash(Handle);
        }

        public static DemoNError ExecXSVF(IntPtr Handle, string Filename)
        {
            return Unmanaged.demon_exec_xsvf(Handle, Filename);
        }

        public static DemoNError PowerOn(IntPtr Handle)
        {
            return Unmanaged.demon_power_on(Handle);
        }

        public static DemoNError PowerOff(IntPtr Handle)
        {
            return Unmanaged.demon_power_off(Handle);
        }

        public static DemoNError SelectFlash(IntPtr Handle, DemoNFlash Flash)
        {
            return Unmanaged.demon_select_flash(Handle, Flash);
        }

        public static DemoNError UpdateFirmware(IntPtr Handle, string Filename)
        {
            return Unmanaged.demon_update_firmware(Handle, Filename);
        }

        public static string GetErrorMessage(DemoNError Error)
        {
            return Unmanaged.demon_get_error_message(Error);
        }
    }
}