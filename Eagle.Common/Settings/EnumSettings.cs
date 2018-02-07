using System;

namespace Eagle.Common.Settings
{
    public enum ColorOption
    {
        Red,
        Green,
        Blue,
        BrightRed,
        BrightGreen,
        BrightBlue,
    }
    public enum ExportFile
    {
        Word,
        Excel,
        Pdf
    }
    public enum Process
    {
        Order = 1, // Bán Hàng
        Warranty = 2, // Trả Hàng
        Purchase = 3, // Mua Hàng
        ChangePrice = 4, // Đổi Giá
        BalanceStock = 5, // Cập nhật Đầu Kỳ
        FactInventory = 6, // Cập Nhật Thực Tế
        TransferStock = 7, // Chuyển Kho
        DefectStock = 8, // Bỏ Hàng
        LiquidationStock = 9 // Thanh Lý
    }
    public enum WeekDay
    {
        Sunday,
        Monday,
        Tuesday,
        Wednesday,
        Thursday,
        Friday,
        Saturday
    }

    [Flags]
    public enum PublishState
    {
        Draft = 0,
        Waiting = 1,
        Processing = 2,
        Published = 4,
        Error = 8
    }
}
