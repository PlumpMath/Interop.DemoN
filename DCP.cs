/*
Copyright (c) 2013 Nathan LeRoux

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/
using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xecuter.DCP
{
    public enum DCPFWStat : uint
    {
        OK = 0,
        NotPresent
    }

    public enum DCPMode : uint
    {
        Bootloader = 0,
        Firmware
    }

    public enum DCPExtFlash : uint
    {
        Xbox360 = 0,
        DemoN
    }

    public enum DCPError : uint
    {
        None = 0,
        OutOfMemory,
        DeviceInit,
        DeviceNotOpen,
        DeviceOpen,
        DeviceRead,
        DeviceWrite,
        DeviceIllegalValue,
        FlashMaxBadBlocks,
        XSVFUnknown,
        XSVFTDOMismatch,
        XSVFMaxRetries,
        XSVFIllegalCommand,
        XSVFIllegalState,
        XSVFDataOverflow,
        XSVFRead
    }

    public enum DCPDeviceID : ushort
    {
        Phat16MB = 0,
        Slim16MB
    }
    
    [StructLayout(LayoutKind.Sequential)]
    public struct DCPBadBlockInfo
    {
        public void Init()
        {
            Count = 0;
            Blocks = new ushort[64];
        }

        public ushort Count;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst=64)]
        public ushort[] Blocks;
    }

    static class Unmanaged
    {
        [DllImport("dcp.dll", EntryPoint = "dcp_create", CallingConvention = CallingConvention.Cdecl)]
        public static extern DCPError dcp_create(out IntPtr self);

        [DllImport("dcp.dll", EntryPoint = "dcp_destroy", CallingConvention = CallingConvention.Cdecl)]
        public static extern void dcp_destroy(IntPtr self);

        [DllImport("dcp.dll", EntryPoint = "dcp_open", CallingConvention = CallingConvention.Cdecl)]
        public static extern DCPError dcp_open(IntPtr self);

        [DllImport("dcp.dll", EntryPoint = "dcp_close", CallingConvention = CallingConvention.Cdecl)]
        public static extern void dcp_close(IntPtr self);

        [DllImport("dcp.dll", EntryPoint = "dcp_get_mode", CallingConvention = CallingConvention.Cdecl)]
        public static extern DCPError dcp_get_mode(IntPtr self, out DCPMode mode);

        [DllImport("dcp.dll", EntryPoint = "dcp_get_proto_ver", CallingConvention = CallingConvention.Cdecl)]
        public static extern DCPError dcp_get_proto_ver(IntPtr self, out ushort ver);

        [DllImport("dcp.dll", EntryPoint = "dcp_get_device_id", CallingConvention = CallingConvention.Cdecl)]
        public static extern DCPError dcp_get_device_id(IntPtr self, out DCPDeviceID ver);

        [DllImport("dcp.dll", EntryPoint = "dcp_get_firmware_ver", CallingConvention = CallingConvention.Cdecl)]
        public static extern DCPError dcp_get_firmware_ver(IntPtr self, out ushort ver);

        [DllImport("dcp.dll", EntryPoint = "dcp_run_bootloader", CallingConvention = CallingConvention.Cdecl)]
        public static extern DCPError dcp_run_bootloader(IntPtr self);

        [DllImport("dcp.dll", EntryPoint = "dcp_get_ext_flash", CallingConvention = CallingConvention.Cdecl)]
        public static extern DCPError dcp_get_ext_flash(IntPtr self, out DCPExtFlash flash);

        [DllImport("dcp.dll", EntryPoint = "dcp_set_ext_flash", CallingConvention = CallingConvention.Cdecl)]
        public static extern DCPError dcp_set_ext_flash(IntPtr self, DCPExtFlash flash);

        [DllImport("dcp.dll", EntryPoint = "dcp_acquire_ext_flash", CallingConvention = CallingConvention.Cdecl)]
        public static extern DCPError dcp_acquire_ext_flash(IntPtr self);

        [DllImport("dcp.dll", EntryPoint = "dcp_release_ext_flash", CallingConvention = CallingConvention.Cdecl)]
        public static extern DCPError dcp_release_ext_flash(IntPtr self);

        [DllImport("dcp.dll", EntryPoint = "dcp_get_ext_flash_id", CallingConvention = CallingConvention.Cdecl)]
        public static extern DCPError dcp_get_ext_flash_id(IntPtr self, out ushort id);

        [DllImport("dcp.dll", EntryPoint = "dcp_get_bad_blocks", CallingConvention = CallingConvention.Cdecl)]
        public static extern DCPError dcp_get_bad_blocks(IntPtr self, ref DCPBadBlockInfo info);

        [DllImport("dcp.dll", EntryPoint = "dcp_erase_ext_flash_block", CallingConvention = CallingConvention.Cdecl)]
        public static extern DCPError dcp_erase_ext_flash_block(IntPtr self, ushort block);

        [DllImport("dcp.dll", EntryPoint = "dcp_erase_all_ext_flash_blocks", CallingConvention = CallingConvention.Cdecl)]
        public static extern DCPError dcp_erase_all_ext_flash_blocks(IntPtr self);

        [DllImport("dcp.dll", EntryPoint = "dcp_read_ext_flash_block", CallingConvention = CallingConvention.Cdecl)]
        public static extern DCPError dcp_read_ext_flash_block(IntPtr self, ushort block, uint size, byte[] buf);

        [DllImport("dcp.dll", EntryPoint = "dcp_program_ext_flash_block", CallingConvention = CallingConvention.Cdecl)]
        public static extern DCPError dcp_program_ext_flash_block(IntPtr self, ushort block, uint size, byte[] buf);

        [DllImport("dcp.dll", EntryPoint = "dcp_assert_sb_reset", CallingConvention = CallingConvention.Cdecl)]
        public static extern DCPError dcp_assert_sb_reset(IntPtr self);

        [DllImport("dcp.dll", EntryPoint = "dcp_deassert_sb_reset", CallingConvention = CallingConvention.Cdecl)]
        public static extern DCPError dcp_deassert_sb_reset(IntPtr self);

        [DllImport("dcp.dll", EntryPoint = "dcp_read_serial", CallingConvention = CallingConvention.Cdecl)]
        public static extern DCPError dcp_read_serial(IntPtr self, out ushort num, byte[] buf);

        [DllImport("dcp.dll", EntryPoint = "dcp_exec_xsvf_begin", CallingConvention = CallingConvention.Cdecl)]
        public static extern DCPError dcp_exec_xsvf_begin(IntPtr self, uint size);

        [DllImport("dcp.dll", EntryPoint = "dcp_exec_xsvf_next", CallingConvention = CallingConvention.Cdecl)]
        public static extern DCPError dcp_exec_xsvf_next(IntPtr self, uint size, byte[] buf);

        [DllImport("dcp.dll", EntryPoint = "dcp_exec_xsvf_end", CallingConvention = CallingConvention.Cdecl)]
        public static extern DCPError dcp_exec_xsvf_end(IntPtr self);

        [DllImport("dcp.dll", EntryPoint = "dcp_power_on", CallingConvention = CallingConvention.Cdecl)]
        public static extern DCPError dcp_power_on(IntPtr self);

        [DllImport("dcp.dll", EntryPoint = "dcp_power_off", CallingConvention = CallingConvention.Cdecl)]
        public static extern DCPError dcp_power_off(IntPtr self);

        [DllImport("dcp.dll", EntryPoint = "dcp_get_bootloader_ver", CallingConvention = CallingConvention.Cdecl)]
        public static extern DCPError dcp_get_bootloader_ver(IntPtr self, out ushort ver);

        [DllImport("dcp.dll", EntryPoint = "dcp_run_firmware", CallingConvention = CallingConvention.Cdecl)]
        public static extern DCPError dcp_run_firmware(IntPtr self);

        [DllImport("dcp.dll", EntryPoint = "dcp_check_firmware_status", CallingConvention = CallingConvention.Cdecl)]
        public static extern DCPError dcp_check_firmware_status(IntPtr self, out DCPFWStat stat);

        [DllImport("dcp.dll", EntryPoint = "dcp_update_firmware_begin", CallingConvention = CallingConvention.Cdecl)]
        public static extern DCPError dcp_update_firmware_begin(IntPtr self);

        [DllImport("dcp.dll", EntryPoint = "dcp_update_firmware_end", CallingConvention = CallingConvention.Cdecl)]
        public static extern DCPError dcp_update_firmware_end(IntPtr self);

        [DllImport("dcp.dll", EntryPoint = "dcp_read_int_flash_page", CallingConvention = CallingConvention.Cdecl)]
        public static extern DCPError dcp_read_int_flash_page(IntPtr self, byte page, byte[] buf);

        [DllImport("dcp.dll", EntryPoint = "dcp_erase_int_flash_page", CallingConvention = CallingConvention.Cdecl)]
        public static extern DCPError dcp_read_erase_int_flash_page(IntPtr self, byte page);

        [DllImport("dcp.dll", EntryPoint = "dcp_program_int_flash_page", CallingConvention = CallingConvention.Cdecl)]
        public static extern DCPError dcp_read_program_int_flash_page(IntPtr self, byte page, byte[] buf);
    }

    public static class DCP
    {
        public static DCPError Create(out IntPtr Handle)
        {
            return Unmanaged.dcp_create(out Handle);
        }

        public static void Destroy(IntPtr Handle)
        {
            Unmanaged.dcp_destroy(Handle);
        }

        public static DCPError Open(IntPtr Handle)
        {
            return Unmanaged.dcp_open(Handle);
        }

        public static void Close(IntPtr Handle)
        {
            Unmanaged.dcp_close(Handle);
        }

        public static DCPError GetMode(IntPtr Handle, out DCPMode Mode)
        {
            return Unmanaged.dcp_get_mode(Handle, out Mode);
        }

        public static DCPError GetProtocolVersion(IntPtr Handle, out ushort Version)
        {
            return Unmanaged.dcp_get_proto_ver(Handle, out Version);
        }

        public static DCPError GetDeviceID(IntPtr Handle, out DCPDeviceID ID)
        {
            return Unmanaged.dcp_get_device_id(Handle, out ID);
        }

        public static DCPError GetFirmwareVersion(IntPtr Handle, out ushort Version)
        {
            return Unmanaged.dcp_get_firmware_ver(Handle, out Version);
        }

        public static DCPError RunBootloader(IntPtr Handle)
        {
            return Unmanaged.dcp_run_bootloader(Handle);
        }

        public static DCPError GetExtFlash(IntPtr Handle, out DCPExtFlash Flash)
        {
            return Unmanaged.dcp_get_ext_flash(Handle, out Flash);
        }

        public static DCPError SetExtFlash(IntPtr Handle, DCPExtFlash Flash)
        {
            return Unmanaged.dcp_set_ext_flash(Handle, Flash);
        }

        public static DCPError AcquireExtFlash(IntPtr Handle)
        {
            return Unmanaged.dcp_acquire_ext_flash(Handle);
        }

        public static DCPError ReleaseExtFlash(IntPtr Handle)
        {
            return Unmanaged.dcp_release_ext_flash(Handle);
        }

        public static DCPError GetExtFlashID(IntPtr Handle, out ushort ID)
        {
            return Unmanaged.dcp_get_ext_flash_id(Handle, out ID);
        }

        public static DCPError GetBadBlocks(IntPtr Handle, out DCPBadBlockInfo Info)
        {
            DCPBadBlockInfo info = new DCPBadBlockInfo();
            info.Init();

            DCPError err = Unmanaged.dcp_get_bad_blocks(Handle, ref info);

            Info = info;
            return err;
        }

        public static DCPError EraseExtFlashBlock(IntPtr Handle, ushort Block)
        {
            return Unmanaged.dcp_erase_ext_flash_block(Handle, Block);
        }

        public static DCPError EraseAllExtFlashBlocks(IntPtr Handle)
        {
            return Unmanaged.dcp_erase_all_ext_flash_blocks(Handle);
        }

        public static DCPError ReadExtFlashBlock(IntPtr Handle, ushort Block, uint Size, out byte[] Buffer)
        {
            byte[] buffer = new byte[Size];

            DCPError err = Unmanaged.dcp_read_ext_flash_block(Handle, Block, Size, buffer);

            if (err == DCPError.None)
                Buffer = buffer;
            else
                Buffer = null;

            return err;
        }

        public static DCPError ProgramExtFlashBlock(IntPtr Handle, ushort Block, uint Size, byte[] Buffer)
        {
            return Unmanaged.dcp_program_ext_flash_block(Handle, Block, Size, Buffer);
        }

        public static DCPError AssertSBReset(IntPtr Handle)
        {
            return Unmanaged.dcp_assert_sb_reset(Handle);
        }

        public static DCPError DeassertSBReset(IntPtr Handle)
        {
            return Unmanaged.dcp_deassert_sb_reset(Handle);
        }

        public static DCPError ReadSerial(IntPtr Handle, out byte[] Buffer)
        {
            byte[] buffer = new byte[1536];
            ushort count;

            DCPError err = Unmanaged.dcp_read_serial(Handle, out count, buffer);

            if (err != DCPError.None || count == 0)
                Buffer = null;
            else if (count != buffer.Length)
            {
                Buffer = new byte[count];
                Array.Copy(buffer, Buffer, count);
            }
            else
                Buffer = buffer;

            return err;
        }

        public static DCPError ExecXSVFBegin(IntPtr Handle, uint Size)
        {
            return Unmanaged.dcp_exec_xsvf_begin(Handle, Size);
        }

        public static DCPError ExecXSVFNext(IntPtr Handle, uint Size, byte[] Buffer)
        {
            return Unmanaged.dcp_exec_xsvf_next(Handle, Size, Buffer);
        }

        public static DCPError ExecXSVFEnd(IntPtr Handle)
        {
            return Unmanaged.dcp_exec_xsvf_end(Handle);
        }

        public static DCPError PowerOn(IntPtr Handle)
        {
            return Unmanaged.dcp_power_on(Handle);
        }

        public static DCPError PowerOff(IntPtr Handle)
        {
            return Unmanaged.dcp_power_off(Handle);
        }

        public static DCPError GetBootloaderVersion(IntPtr Handle, out ushort Version)
        {
            return Unmanaged.dcp_get_bootloader_ver(Handle, out Version);
        }

        public static DCPError RunFirmware(IntPtr Handle)
        {
            return Unmanaged.dcp_run_firmware(Handle);
        }

        public static DCPError CheckFirmwareStatus(IntPtr Handle, out DCPFWStat Status)
        {
            return Unmanaged.dcp_check_firmware_status(Handle, out Status);
        }

        public static DCPError UpdateFirmwareBegin(IntPtr Handle)
        {
            return Unmanaged.dcp_update_firmware_begin(Handle);
        }

        public static DCPError UpdateFirmwareEnd(IntPtr Handle)
        {
            return Unmanaged.dcp_update_firmware_end(Handle);
        }

        public static DCPError ReadIntFlashPage(IntPtr Handle, byte Page, out byte[] Buffer)
        {
            Buffer = new byte[256];
            return Unmanaged.dcp_read_int_flash_page(Handle, Page, Buffer);
        }

        public static DCPError EraseIntFlashPage(IntPtr Handle, byte Page)
        {
            return Unmanaged.dcp_read_erase_int_flash_page(Handle, Page);
        }

        public static DCPError ProgramIntFlashPage(IntPtr Handle, byte Page, byte[] Buffer)
        {
            return Unmanaged.dcp_read_program_int_flash_page(Handle, Page, Buffer);
        }
    }
}
