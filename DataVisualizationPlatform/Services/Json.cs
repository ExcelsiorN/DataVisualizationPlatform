namespace DataVisualizationPlatform.Services
{
    internal class Json
    {
        public readonly string _EquipmentInfo = @"[
            {
                ""Equ_Id"": ""fntp-0"",
                ""Equ_Name"": ""A320-NEO-FNPT"",
                ""Equ_OnlineStatus"": ""在线"",
                ""Equ_AvailableBookingPeriod"": ""预约时段配置2"",
                ""Equ_TotalOperationTime"": ""0年4月6天"",
                ""Equ_FixedDurationThisYear"": ""2920.0小时"",
                ""Equ_UsedFixedDurationThisYear"": ""0.0小时"",
                ""Equ_UsageRateThisYear"": ""0.0%"",
                ""Equ_DeploymentAddress"": ""local test""
            },
            {
                ""Equ_Id"": ""fntp-1"",
                ""Equ_Name"": ""B737-MAX-FNPT"",
                ""Equ_OnlineStatus"": ""在线"",
                ""Equ_AvailableBookingPeriod"": ""预约时段配置1"",
                ""Equ_TotalOperationTime"": ""1年2月15天"",
                ""Equ_FixedDurationThisYear"": ""3500.0小时"",
                ""Equ_UsedFixedDurationThisYear"": ""1250.5小时"",
                ""Equ_UsageRateThisYear"": ""35.7%"",
                ""Equ_DeploymentAddress"": ""local test""
            },
            {
                ""Equ_Id"": ""fntp-2"",
                ""Equ_Name"": ""A320-NEO-FNPT"",
                ""Equ_OnlineStatus"": ""离线"",
                ""Equ_AvailableBookingPeriod"": ""预约时段配置3"",
                ""Equ_TotalOperationTime"": ""0年8月20天"",
                ""Equ_FixedDurationThisYear"": ""1800.0小时"",
                ""Equ_UsedFixedDurationThisYear"": ""900.0小时"",
                ""Equ_UsageRateThisYear"": ""50.0%"",
                ""Equ_DeploymentAddress"": ""local test""
            },
            {
                ""Equ_Id"": ""fntp-3"",
                ""Equ_Name"": ""A320-NEO-FNPT"",
                ""Equ_OnlineStatus"": ""在线"",
                ""Equ_AvailableBookingPeriod"": ""预约时段配置2"",
                ""Equ_TotalOperationTime"": ""3年0月10天"",
                ""Equ_FixedDurationThisYear"": ""5000.0小时"",
                ""Equ_UsedFixedDurationThisYear"": ""3750.0小时"",
                ""Equ_UsageRateThisYear"": ""75.0%"",
                ""Equ_DeploymentAddress"": ""local test""
            },
            {
                ""Equ_Id"": ""fntp-4"",
                ""Equ_Name"": ""A320-NEO-FNPT"",
                ""Equ_OnlineStatus"": ""在线"",
                ""Equ_AvailableBookingPeriod"": ""预约时段配置4"",
                ""Equ_TotalOperationTime"": ""2年6月5天"",
                ""Equ_FixedDurationThisYear"": ""4000.0小时"",
                ""Equ_UsedFixedDurationThisYear"": ""2000.0小时"",
                ""Equ_UsageRateThisYear"": ""50.0%"",
                ""Equ_DeploymentAddress"": ""local test""
            },
            {
                ""Equ_Id"": ""fntp-5"",
                ""Equ_Name"": ""A320-NEO-FNPT"",
                ""Equ_OnlineStatus"": ""离线"",
                ""Equ_AvailableBookingPeriod"": ""预约时段配置1"",
                ""Equ_TotalOperationTime"": ""1年1月1天"",
                ""Equ_FixedDurationThisYear"": ""2500.0小时"",
                ""Equ_UsedFixedDurationThisYear"": ""1000.0小时"",
                ""Equ_UsageRateThisYear"": ""40.0%"",
                ""Equ_DeploymentAddress"": ""local test""
            }
        ]";

        public readonly string _ReservationList = @"[
            {
                ""Res_ID"": ""1"",
                ""Res_OrderID"": ""order_001"",
                ""Res_Equipment"": ""fntp-0"",
                ""Res_Date"": ""March 28, 2000"",
                ""Res_Start"": ""5 p.m."",
                ""Res_End"": ""6 p.m."",
                ""Res_PaymentStatus"": ""待支付"",
                ""Res_PaymentInfo"": ""-"",
                ""Res_Status"": ""未完成"",
                ""Res_Detail"": ""-"",
                ""Res_StartDate"": ""April 28, 2025, 1 p.m.""
            },
            {
                ""Res_ID"": ""2"",
                ""Res_OrderID"": ""order_002"",
                ""Res_Equipment"": ""fntp-1"",
                ""Res_Date"": ""May 28, 2001"",
                ""Res_Start"": ""7 p.m."",
                ""Res_End"": ""9 p.m."",
                ""Res_PaymentStatus"": ""待支付"",
                ""Res_PaymentInfo"": ""-"",
                ""Res_Status"": ""未完成"",
                ""Res_Detail"": ""-"",
                ""Res_StartDate"": ""April 28, 2025, 2 p.m.""
            },
            {
                ""Res_ID"": ""3"",
                ""Res_OrderID"": ""order_003"",
                ""Res_Equipment"": ""fntp-2"",
                ""Res_Date"": ""April 29, 2002"",
                ""Res_Start"": ""12 p.m."",
                ""Res_End"": ""1 p.m."",
                ""Res_PaymentStatus"": ""待支付"",
                ""Res_PaymentInfo"": ""-"",
                ""Res_Status"": ""未完成"",
                ""Res_Detail"": ""-"",
                ""Res_StartDate"": ""April 29, 2025, 9 a.m.""
            },
            {
                ""Res_ID"": ""4"",
                ""Res_OrderID"": ""order_004"",
                ""Res_Equipment"": ""fntp-0"",
                ""Res_Date"": ""April 29, 2003"",
                ""Res_Start"": ""3 p.m."",
                ""Res_End"": ""4 p.m."",
                ""Res_PaymentStatus"": ""待支付"",
                ""Res_PaymentInfo"": ""-"",
                ""Res_Status"": ""未完成"",
                ""Res_Detail"": ""-"",
                ""Res_StartDate"": ""April 29, 2025, 10 a.m.""
            },
            {
                ""Res_ID"": ""5"",
                ""Res_OrderID"": ""order_005"",
                ""Res_Equipment"": ""fntp-1"",
                ""Res_Date"": ""April 30, 2004"",
                ""Res_Start"": ""6 p.m."",
                ""Res_End"": ""8 p.m."",
                ""Res_PaymentStatus"": ""待支付"",
                ""Res_PaymentInfo"": ""-"",
                ""Res_Status"": ""未完成"",
                ""Res_Detail"": ""-"",
                ""Res_StartDate"": ""April 30, 2025, 11 a.m.""
            },
            {
                ""Res_ID"": ""6"",
                ""Res_OrderID"": ""order_006"",
                ""Res_Equipment"": ""fntp-2"",
                ""Res_Date"": ""April 30, 2005"",
                ""Res_Start"": ""2 p.m."",
                ""Res_End"": ""3 p.m."",
                ""Res_PaymentStatus"": ""待支付"",
                ""Res_PaymentInfo"": ""-"",
                ""Res_Status"": ""未完成"",
                ""Res_Detail"": ""-"",
                ""Res_StartDate"": ""April 30, 2025, 8 a.m.""
            },
            {
                ""Res_ID"": ""7"",
                ""Res_OrderID"": ""order_007"",
                ""Res_Equipment"": ""fntp-0"",
                ""Res_Date"": ""May 1, 2006"",
                ""Res_Start"": ""11 a.m."",
                ""Res_End"": ""12 p.m."",
                ""Res_PaymentStatus"": ""待支付"",
                ""Res_PaymentInfo"": ""-"",
                ""Res_Status"": ""未完成"",
                ""Res_Detail"": ""-"",
                ""Res_StartDate"": ""May 1, 2025, 7 a.m.""
            },
            {
                ""Res_ID"": ""8"",
                ""Res_OrderID"": ""order_008"",
                ""Res_Equipment"": ""fntp-1"",
                ""Res_Date"": ""May 1, 2007"",
                ""Res_Start"": ""4 p.m."",
                ""Res_End"": ""6 p.m."",
                ""Res_PaymentStatus"": ""待支付"",
                ""Res_PaymentInfo"": ""-"",
                ""Res_Status"": ""未完成"",
                ""Res_Detail"": ""-"",
                ""Res_StartDate"": ""May 1, 2025, 12 p.m.""
            },
            {
                ""Res_ID"": ""9"",
                ""Res_OrderID"": ""order_009"",
                ""Res_Equipment"": ""fntp-2"",
                ""Res_Date"": ""May 2, 2008"",
                ""Res_Start"": ""7 p.m."",
                ""Res_End"": ""9 p.m."",
                ""Res_PaymentStatus"": ""待支付"",
                ""Res_PaymentInfo"": ""-"",
                ""Res_Status"": ""未完成"",
                ""Res_Detail"": ""-"",
                ""Res_StartDate"": ""May 2, 2025, 2 p.m.""
            },
            {
                ""Res_ID"": ""10"",
                ""Res_OrderID"": ""order_010"",
                ""Res_Equipment"": ""fntp-0"",
                ""Res_Date"": ""May 2, 2025"",
                ""Res_Start"": ""1 p.m."",
                ""Res_End"": ""2 p.m."",
                ""Res_PaymentStatus"": ""待支付"",
                ""Res_PaymentInfo"": ""-"",
                ""Res_Status"": ""未完成"",
                ""Res_Detail"": ""-"",
                ""Res_StartDate"": ""May 2, 2025, 9 a.m.""
            },
            {
                ""Res_ID"": ""11"",
                ""Res_OrderID"": ""order_011"",
                ""Res_Equipment"": ""fntp-1"",
                ""Res_Date"": ""May 3, 2025"",
                ""Res_Start"": ""5 p.m."",
                ""Res_End"": ""7 p.m."",
                ""Res_PaymentStatus"": ""待支付"",
                ""Res_PaymentInfo"": ""-"",
                ""Res_Status"": ""未完成"",
                ""Res_Detail"": ""-"",
                ""Res_StartDate"": ""May 3, 2025, 1 p.m.""
            },
            {
                ""Res_ID"": ""12"",
                ""Res_OrderID"": ""order_012"",
                ""Res_Equipment"": ""fntp-2"",
                ""Res_Date"": ""May 3, 2025"",
                ""Res_Start"": ""12 p.m."",
                ""Res_End"": ""1 p.m."",
                ""Res_PaymentStatus"": ""待支付"",
                ""Res_PaymentInfo"": ""-"",
                ""Res_Status"": ""未完成"",
                ""Res_Detail"": ""-"",
                ""Res_StartDate"": ""May 3, 2025, 8 a.m.""
            },
            {
                ""Res_ID"": ""13"",
                ""Res_OrderID"": ""order_013"",
                ""Res_Equipment"": ""fntp-0"",
                ""Res_Date"": ""May 4, 2025"",
                ""Res_Start"": ""3 p.m."",
                ""Res_End"": ""4 p.m."",
                ""Res_PaymentStatus"": ""待支付"",
                ""Res_PaymentInfo"": ""-"",
                ""Res_Status"": ""未完成"",
                ""Res_Detail"": ""-"",
                ""Res_StartDate"": ""May 4, 2025, 10 a.m.""
            },
            {
                ""Res_ID"": ""14"",
                ""Res_OrderID"": ""order_014"",
                ""Res_Equipment"": ""fntp-1"",
                ""Res_Date"": ""May 4, 2025"",
                ""Res_Start"": ""6 p.m."",
                ""Res_End"": ""8 p.m."",
                ""Res_PaymentStatus"": ""待支付"",
                ""Res_PaymentInfo"": ""-"",
                ""Res_Status"": ""未完成"",
                ""Res_Detail"": ""-"",
                ""Res_StartDate"": ""May 4, 2025, 11 a.m.""
            },
            {
                ""Res_ID"": ""15"",
                ""Res_OrderID"": ""order_015"",
                ""Res_Equipment"": ""fntp-2"",
                ""Res_Date"": ""May 5, 2025"",
                ""Res_Start"": ""2 p.m."",
                ""Res_End"": ""3 p.m."",
                ""Res_PaymentStatus"": ""待支付"",
                ""Res_PaymentInfo"": ""-"",
                ""Res_Status"": ""未完成"",
                ""Res_Detail"": ""-"",
                ""Res_StartDate"": ""May 5, 2025, 8 a.m.""
            },
            {
                ""Res_ID"": ""16"",
                ""Res_OrderID"": ""order_016"",
                ""Res_Equipment"": ""fntp-0"",
                ""Res_Date"": ""May 5, 2025"",
                ""Res_Start"": ""11 a.m."",
                ""Res_End"": ""12 p.m."",
                ""Res_PaymentStatus"": ""待支付"",
                ""Res_PaymentInfo"": ""-"",
                ""Res_Status"": ""未完成"",
                ""Res_Detail"": ""-"",
                ""Res_StartDate"": ""May 5, 2025, 7 a.m.""
            },
            {
                ""Res_ID"": ""17"",
                ""Res_OrderID"": ""order_017"",
                ""Res_Equipment"": ""fntp-1"",
                ""Res_Date"": ""May 6, 2025"",
                ""Res_Start"": ""4 p.m."",
                ""Res_End"": ""6 p.m."",
                ""Res_PaymentStatus"": ""待支付"",
                ""Res_PaymentInfo"": ""-"",
                ""Res_Status"": ""未完成"",
                ""Res_Detail"": ""-"",
                ""Res_StartDate"": ""May 6, 2025, 12 p.m.""
            },
            {
                ""Res_ID"": ""18"",
                ""Res_OrderID"": ""order_018"",
                ""Res_Equipment"": ""fntp-2"",
                ""Res_Date"": ""May 6, 2025"",
                ""Res_Start"": ""7 p.m."",
                ""Res_End"": ""9 p.m."",
                ""Res_PaymentStatus"": ""待支付"",
                ""Res_PaymentInfo"": ""-"",
                ""Res_Status"": ""未完成"",
                ""Res_Detail"": ""-"",
                ""Res_StartDate"": ""May 6, 2025, 2 p.m.""
            },
            {
                ""Res_ID"": ""19"",
                ""Res_OrderID"": ""order_019"",
                ""Res_Equipment"": ""fntp-0"",
                ""Res_Date"": ""May 7, 2025"",
                ""Res_Start"": ""1 p.m."",
                ""Res_End"": ""2 p.m."",
                ""Res_PaymentStatus"": ""待支付"",
                ""Res_PaymentInfo"": ""-"",
                ""Res_Status"": ""未完成"",
                ""Res_Detail"": ""-"",
                ""Res_StartDate"": ""May 7, 2025, 9 a.m.""
            },
            {
                ""Res_ID"": ""20"",
                ""Res_OrderID"": ""order_020"",
                ""Res_Equipment"": ""fntp-1"",
                ""Res_Date"": ""May 7, 2025"",
                ""Res_Start"": ""5 p.m."",
                ""Res_End"": ""7 p.m."",
                ""Res_PaymentStatus"": ""待支付"",
                ""Res_PaymentInfo"": ""-"",
                ""Res_Status"": ""未完成"",
                ""Res_Detail"": ""-"",
                ""Res_StartDate"": ""May 7, 2025, 1 p.m.""
            },
            {
                ""Res_ID"": ""21"",
                ""Res_OrderID"": ""order_021"",
                ""Res_Equipment"": ""fntp-2"",
                ""Res_Date"": ""May 8, 2025"",
                ""Res_Start"": ""12 p.m."",
                ""Res_End"": ""1 p.m."",
                ""Res_PaymentStatus"": ""待支付"",
                ""Res_PaymentInfo"": ""-"",
                ""Res_Status"": ""未完成"",
                ""Res_Detail"": ""-"",
                ""Res_StartDate"": ""May 8, 2025, 8 a.m.""
            },
            {
                ""Res_ID"": ""22"",
                ""Res_OrderID"": ""order_022"",
                ""Res_Equipment"": ""fntp-0"",
                ""Res_Date"": ""May 8, 2025"",
                ""Res_Start"": ""3 p.m."",
                ""Res_End"": ""4 p.m."",
                ""Res_PaymentStatus"": ""待支付"",
                ""Res_PaymentInfo"": ""-"",
                ""Res_Status"": ""未完成"",
                ""Res_Detail"": ""-"",
                ""Res_StartDate"": ""May 8, 2025, 10 a.m.""
            },
            {
                ""Res_ID"": ""23"",
                ""Res_OrderID"": ""order_023"",
                ""Res_Equipment"": ""fntp-1"",
                ""Res_Date"": ""May 9, 2025"",
                ""Res_Start"": ""6 p.m."",
                ""Res_End"": ""8 p.m."",
                ""Res_PaymentStatus"": ""待支付"",
                ""Res_PaymentInfo"": ""-"",
                ""Res_Status"": ""未完成"",
                ""Res_Detail"": ""-"",
                ""Res_StartDate"": ""May 9, 2025, 11 a.m.""
            },
            {
                ""Res_ID"": ""24"",
                ""Res_OrderID"": ""order_024"",
                ""Res_Equipment"": ""fntp-2"",
                ""Res_Date"": ""May 9, 2025"",
                ""Res_Start"": ""2 p.m."",
                ""Res_End"": ""3 p.m."",
                ""Res_PaymentStatus"": ""待支付"",
                ""Res_PaymentInfo"": ""-"",
                ""Res_Status"": ""未完成"",
                ""Res_Detail"": ""-"",
                ""Res_StartDate"": ""May 9, 2025, 8 a.m.""
            },
            {
                ""Res_ID"": ""25"",
                ""Res_OrderID"": ""order_025"",
                ""Res_Equipment"": ""fntp-0"",
                ""Res_Date"": ""May 10, 2025"",
                ""Res_Start"": ""11 a.m."",
                ""Res_End"": ""12 p.m."",
                ""Res_PaymentStatus"": ""待支付"",
                ""Res_PaymentInfo"": ""-"",
                ""Res_Status"": ""未完成"",
                ""Res_Detail"": ""-"",
                ""Res_StartDate"": ""May 10, 2025, 7 a.m.""
            },
            {
                ""Res_ID"": ""26"",
                ""Res_OrderID"": ""order_026"",
                ""Res_Equipment"": ""fntp-1"",
                ""Res_Date"": ""May 10, 2025"",
                ""Res_Start"": ""4 p.m."",
                ""Res_End"": ""6 p.m."",
                ""Res_PaymentStatus"": ""待支付"",
                ""Res_PaymentInfo"": ""-"",
                ""Res_Status"": ""未完成"",
                ""Res_Detail"": ""-"",
                ""Res_StartDate"": ""May 10, 2025, 12 p.m.""
            },
            {
                ""Res_ID"": ""27"",
                ""Res_OrderID"": ""order_027"",
                ""Res_Equipment"": ""fntp-2"",
                ""Res_Date"": ""May 11, 2025"",
                ""Res_Start"": ""7 p.m."",
                ""Res_End"": ""9 p.m."",
                ""Res_PaymentStatus"": ""待支付"",
                ""Res_PaymentInfo"": ""-"",
                ""Res_Status"": ""未完成"",
                ""Res_Detail"": ""-"",
                ""Res_StartDate"": ""May 11, 2025, 2 p.m.""
            },
            {
                ""Res_ID"": ""28"",
                ""Res_OrderID"": ""order_028"",
                ""Res_Equipment"": ""fntp-0"",
                ""Res_Date"": ""May 11, 2025"",
                ""Res_Start"": ""1 p.m."",
                ""Res_End"": ""2 p.m."",
                ""Res_PaymentStatus"": ""待支付"",
                ""Res_PaymentInfo"": ""-"",
                ""Res_Status"": ""未完成"",
                ""Res_Detail"": ""-"",
                ""Res_StartDate"": ""May 11, 2025, 9 a.m.""
            },
            {
                ""Res_ID"": ""29"",
                ""Res_OrderID"": ""order_029"",
                ""Res_Equipment"": ""fntp-1"",
                ""Res_Date"": ""May 12, 2025"",
                ""Res_Start"": ""5 p.m."",
                ""Res_End"": ""7 p.m."",
                ""Res_PaymentStatus"": ""待支付"",
                ""Res_PaymentInfo"": ""-"",
                ""Res_Status"": ""未完成"",
                ""Res_Detail"": ""-"",
                ""Res_StartDate"": ""May 12, 2025, 1 p.m.""
            },
            {
                ""Res_ID"": ""30"",
                ""Res_OrderID"": ""order_030"",
                ""Res_Equipment"": ""fntp-2"",
                ""Res_Date"": ""May 12, 2025"",
                ""Res_Start"": ""12 p.m."",
                ""Res_End"": ""1 p.m."",
                ""Res_PaymentStatus"": ""待支付"",
                ""Res_PaymentInfo"": ""-"",
                ""Res_Status"": ""未完成"",
                ""Res_Detail"": ""-"",
                ""Res_StartDate"": ""May 12, 2025, 8 a.m.""
            },
            {
                ""Res_ID"": ""31"",
                ""Res_OrderID"": ""order_031"",
                ""Res_Equipment"": ""fntp-0"",
                ""Res_Date"": ""May 13, 2025"",
                ""Res_Start"": ""3 p.m."",
                ""Res_End"": ""4 p.m."",
                ""Res_PaymentStatus"": ""待支付"",
                ""Res_PaymentInfo"": ""-"",
                ""Res_Status"": ""未完成"",
                ""Res_Detail"": ""-"",
                ""Res_StartDate"": ""May 13, 2025, 10 a.m.""
            },
            {
                ""Res_ID"": ""32"",
                ""Res_OrderID"": ""order_032"",
                ""Res_Equipment"": ""fntp-1"",
                ""Res_Date"": ""May 13, 2025"",
                ""Res_Start"": ""6 p.m."",
                ""Res_End"": ""8 p.m."",
                ""Res_PaymentStatus"": ""待支付"",
                ""Res_PaymentInfo"": ""-"",
                ""Res_Status"": ""未完成"",
                ""Res_Detail"": ""-"",
                ""Res_StartDate"": ""May 13, 2025, 11 a.m.""
            },
            {
                ""Res_ID"": ""33"",
                ""Res_OrderID"": ""order_033"",
                ""Res_Equipment"": ""fntp-2"",
                ""Res_Date"": ""May 14, 2025"",
                ""Res_Start"": ""2 p.m."",
                ""Res_End"": ""3 p.m."",
                ""Res_PaymentStatus"": ""待支付"",
                ""Res_PaymentInfo"": ""-"",
                ""Res_Status"": ""未完成"",
                ""Res_Detail"": ""-"",
                ""Res_StartDate"": ""May 14, 2025, 8 a.m.""
            },
            {
                ""Res_ID"": ""34"",
                ""Res_OrderID"": ""order_034"",
                ""Res_Equipment"": ""fntp-0"",
                ""Res_Date"": ""May 14, 2025"",
                ""Res_Start"": ""11 a.m."",
                ""Res_End"": ""12 p.m."",
                ""Res_PaymentStatus"": ""待支付"",
                ""Res_PaymentInfo"": ""-"",
                ""Res_Status"": ""未完成"",
                ""Res_Detail"": ""-"",
                ""Res_StartDate"": ""May 14, 2025, 7 a.m.""
            },
            {
                ""Res_ID"": ""35"",
                ""Res_OrderID"": ""order_035"",
                ""Res_Equipment"": ""fntp-1"",
                ""Res_Date"": ""May 15, 2025"",
                ""Res_Start"": ""4 p.m."",
                ""Res_End"": ""6 p.m."",
                ""Res_PaymentStatus"": ""待支付"",
                ""Res_PaymentInfo"": ""-"",
                ""Res_Status"": ""未完成"",
                ""Res_Detail"": ""-"",
                ""Res_StartDate"": ""May 15, 2025, 12 p.m.""
            },
            {
                ""Res_ID"": ""36"",
                ""Res_OrderID"": ""cs_captain"",
                ""Res_Equipment"": ""fntp-0"",
                ""Res_Date"": ""April 30, 2025"",
                ""Res_Start"": ""7 p.m."",
                ""Res_End"": ""8 p.m."",
                ""Res_PaymentStatus"": ""待支付"",
                ""Res_PaymentInfo"": ""-"",
                ""Res_Status"": ""未完成"",
                ""Res_Detail"": ""-"",
                ""Res_StartDate"": ""April 30, 2025, 3 p.m.""
            },
            {
                ""Res_ID"": ""37"",
                ""Res_OrderID"": ""wechat_SYYY84"",
                ""Res_Equipment"": ""fntp-1"",
                ""Res_Date"": ""May 1, 2025"",
                ""Res_Start"": ""9 a.m."",
                ""Res_End"": ""10 a.m."",
                ""Res_PaymentStatus"": ""已支付"",
                ""Res_PaymentInfo"": ""-"",
                ""Res_Status"": ""未完成"",
                ""Res_Detail"": ""-"",
                ""Res_StartDate"": ""May 1, 2025, 8 a.m.""
            },
            {
                ""Res_ID"": ""38"",
                ""Res_OrderID"": ""wechat_SYYY84"",
                ""Res_Equipment"": ""fntp-2"",
                ""Res_Date"": ""May 2, 2025"",
                ""Res_Start"": ""2 p.m."",
                ""Res_End"": ""3 p.m."",
                ""Res_PaymentStatus"": ""待支付"",
                ""Res_PaymentInfo"": ""-"",
                ""Res_Status"": ""已取消"",
                ""Res_Detail"": ""-"",
                ""Res_StartDate"": ""May 2, 2025, 1 p.m.""
            },
            {
                ""Res_ID"": ""39"",
                ""Res_OrderID"": ""wechat_SYYY84"",
                ""Res_Equipment"": ""fntp-3"",
                ""Res_Date"": ""May 3, 2025"",
                ""Res_Start"": ""6 p.m."",
                ""Res_End"": ""7 p.m."",
                ""Res_PaymentStatus"": ""已支付"",
                ""Res_PaymentInfo"": ""-"",
                ""Res_Status"": ""已取消"",
                ""Res_Detail"": ""-"",
                ""Res_StartDate"": ""May 3, 2025, 5 p.m.""
            },
            {
                ""Res_ID"": ""40"",
                ""Res_OrderID"": ""wechat_SYYY84"",
                ""Res_Equipment"": ""fntp-4"",
                ""Res_Date"": ""May 4, 2025"",
                ""Res_Start"": ""10 a.m."",
                ""Res_End"": ""11 p.m."",
                ""Res_PaymentStatus"": ""未支付"",
                ""Res_PaymentInfo"": ""-"",
                ""Res_Status"": ""已完成"",
                ""Res_Detail"": ""-"",
                ""Res_StartDate"": ""May 4, 2025, 9 a.m.""
            },
            {
                ""Res_ID"": ""41"",
                ""Res_OrderID"": ""wechat_SYYY84"",
                ""Res_Equipment"": ""fntp-5"",
                ""Res_Date"": ""May 5, 2025"",
                ""Res_Start"": ""3 p.m."",
                ""Res_End"": ""4 p.m."",
                ""Res_PaymentStatus"": ""待支付"",
                ""Res_PaymentInfo"": ""-"",
                ""Res_Status"": ""未完成"",
                ""Res_Detail"": ""-"",
                ""Res_StartDate"": ""May 5, 2025, 2 p.m.""
            }
        ]";

        public readonly string _FaultReport = @"[
            {
                ""Fal_Id"": ""fntp-0"",
                ""Fal_Info"": ""这里坏了"",
                ""Fal_Data"": ""2025-04""
            },
            {
                ""Fal_Id"": ""fntp-1"",
                ""Fal_Info"": ""那里坏了"",
                ""Fal_Data"": ""2025-03""
            },
            {
                ""Fal_Id"": ""fntp-2"",
                ""Fal_Info"": ""这里也坏了"",
                ""Fal_Data"": ""2025-05""
            },
            {
                ""Fal_Id"": ""fntp-3"",
                ""Fal_Info"": ""那里也坏了"",
                ""Fal_Data"": ""2025-04""
            },
            {
                ""Fal_Id"": ""fntp-4"",
                ""Fal_Info"": ""不知道哪里坏了"",
                ""Fal_Data"": ""2025-03""
            },
            {
                ""Fal_Id"": ""fntp-5"",
                ""Fal_Info"": ""全坏了"",
                ""Fal_Data"": ""2025-04""
            },
            {
                ""Fal_Id"": ""fntp-4"",
                ""Fal_Info"": ""不知道哪里坏了"",
                ""Fal_Data"": ""May 2025""
            }

        ]";
        // 添加公共访问方法
        public string GetEquipmentInfoJson() => _EquipmentInfo;
        public string GetReservationListJson() => _ReservationList;
        public string GetFaultReportJson() => _FaultReport;


    }
}