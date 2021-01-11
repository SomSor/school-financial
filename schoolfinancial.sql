-- phpMyAdmin SQL Dump
-- version 4.9.1
-- https://www.phpmyadmin.net/
--
-- Host: localhost
-- Generation Time: Jan 11, 2021 at 03:54 PM
-- Server version: 8.0.17
-- PHP Version: 7.3.10

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `schoolfinancial`
--

-- --------------------------------------------------------

--
-- Table structure for table `bankaccount`
--

CREATE TABLE `bankaccount` (
  `Id` int(11) NOT NULL,
  `BankName` varchar(255) COLLATE utf8mb4_general_ci NOT NULL,
  `AccountName` varchar(255) COLLATE utf8mb4_general_ci NOT NULL,
  `AccountNumber` varchar(20) COLLATE utf8mb4_general_ci NOT NULL,
  `SchoolId` int(11) NOT NULL,
  `CreatedDate` datetime NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `bankaccount`
--

INSERT INTO `bankaccount` (`Id`, `BankName`, `AccountName`, `AccountNumber`, `SchoolId`, `CreatedDate`) VALUES
(1, 'ธนาคาร1', 'สีส้ม', '1111111111', 1, '2020-12-07 12:44:03'),
(2, 'ธนาคาร2', 'สีน้ำเงิน', '2222222222', 1, '2020-12-07 12:44:03'),
(3, 'ธนาคาร3', 'สีเหลือง', '3333333333', 1, '2020-12-07 12:44:53');

-- --------------------------------------------------------

--
-- Table structure for table `bringforward`
--

CREATE TABLE `bringforward` (
  `Id` int(11) NOT NULL,
  `Amount` decimal(10,2) NOT NULL,
  `Month` date NOT NULL,
  `BudgetId` int(11) NOT NULL,
  `CreatedDate` datetime NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `bringforward`
--

INSERT INTO `bringforward` (`Id`, `Amount`, `Month`, `BudgetId`, `CreatedDate`) VALUES
(8, '1100.91', '2019-11-09', 7, '2020-11-09 15:46:30'),
(9, '223600.91', '2019-12-01', 7, '2020-11-09 15:49:11'),
(10, '81200.91', '2020-01-06', 7, '2020-11-09 15:59:43'),
(11, '277000.91', '2020-02-03', 7, '2020-11-10 15:49:26'),
(16, '50903.13', '2019-11-01', 8, '2020-11-28 16:19:41'),
(17, '384009.35', '2019-12-07', 8, '2020-11-28 17:11:46'),
(18, '277000.91', '2020-03-01', 7, '2020-11-30 11:47:44'),
(19, '277000.91', '2020-04-01', 7, '2020-11-30 11:47:45'),
(20, '277000.91', '2020-05-01', 7, '2020-11-30 11:47:45'),
(21, '277000.91', '2020-06-01', 7, '2020-11-30 11:47:45'),
(22, '277000.91', '2020-07-01', 7, '2020-11-30 11:47:45'),
(23, '277000.91', '2020-08-01', 7, '2020-11-30 11:47:45'),
(24, '277000.91', '2020-09-01', 7, '2020-11-30 11:47:46'),
(25, '277000.91', '2020-10-01', 7, '2020-11-30 11:47:46'),
(26, '277000.91', '2020-11-01', 7, '2020-11-30 11:47:48'),
(27, '381696.04', '2020-01-01', 8, '2020-11-30 12:02:13'),
(28, '381696.04', '2020-02-01', 8, '2020-11-30 12:02:13'),
(29, '381696.04', '2020-03-01', 8, '2020-11-30 12:02:13'),
(30, '381696.04', '2020-04-01', 8, '2020-11-30 12:02:13'),
(31, '381696.04', '2020-05-01', 8, '2020-11-30 12:02:13'),
(32, '381696.04', '2020-06-01', 8, '2020-11-30 12:02:13'),
(33, '381696.04', '2020-07-01', 8, '2020-11-30 12:02:13'),
(34, '381696.04', '2020-08-01', 8, '2020-11-30 12:02:13'),
(35, '381696.04', '2020-09-01', 8, '2020-11-30 12:02:13'),
(36, '381696.04', '2020-10-01', 8, '2020-11-30 12:02:13'),
(37, '381696.04', '2020-11-01', 8, '2020-11-30 12:02:13'),
(38, '277000.91', '2020-12-09', 7, '2020-12-01 14:29:04'),
(39, '-1157.30', '2020-02-08', 0, '2020-12-06 14:29:04'),
(40, '-1157.30', '2020-03-08', 0, '2020-12-06 14:29:04'),
(41, '-1157.30', '2020-04-08', 0, '2020-12-06 14:29:04'),
(42, '-1157.30', '2020-05-08', 0, '2020-12-06 14:29:04'),
(43, '-1157.30', '2020-06-08', 0, '2020-12-06 14:29:04'),
(44, '-1157.30', '2020-07-08', 0, '2020-12-06 14:29:04'),
(45, '-1157.30', '2020-08-08', 0, '2020-12-06 14:29:04'),
(46, '-1157.30', '2020-09-08', 0, '2020-12-06 14:29:04'),
(47, '-1157.30', '2020-10-08', 0, '2020-12-06 14:29:04'),
(48, '-1157.30', '2020-11-08', 0, '2020-12-06 14:29:04'),
(49, '-1157.30', '2020-12-08', 0, '2020-12-06 14:29:04'),
(50, '381696.04', '2020-12-23', 8, '2020-12-06 14:34:01'),
(12216, '0.00', '2019-11-01', 0, '2020-12-06 15:05:57'),
(12217, '0.00', '2019-12-01', 0, '2020-12-06 15:05:57'),
(12218, '0.00', '2020-01-01', 0, '2020-12-06 15:05:57');

-- --------------------------------------------------------

--
-- Table structure for table `budget`
--

CREATE TABLE `budget` (
  `Id` int(11) NOT NULL,
  `Name` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `BankAccountId` int(11) NOT NULL,
  `SchoolId` int(11) NOT NULL,
  `CreatedDate` datetime NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `budget`
--

INSERT INTO `budget` (`Id`, `Name`, `BankAccountId`, `SchoolId`, `CreatedDate`) VALUES
(7, 'อาหารกลางวัน', 3, 1, '2020-10-21 00:00:00'),
(8, 'อุดหนุนรายหัว', 3, 1, '2020-10-21 00:00:00'),
(9, 'นักเรียนประจำพักนอน', 2, 1, '2020-10-21 00:00:00'),
(11, 'เงินประกันสัญญา', 1, 1, '2020-11-03 13:45:39'),
(12, 'เรียนฟรี 15 ปี -อุปกรณ์', 2, 1, '2020-11-03 13:45:57'),
(13, 'เรียนฟรี 15 ปี -หนังสือ', 2, 1, '2020-11-03 13:46:11'),
(14, 'เรียนฟรี 15 ปี -เครื่องแบบ', 2, 1, '2020-11-03 13:46:20'),
(15, 'เรียนฟรี 15 ปี -กิจกรรม', 3, 1, '2020-11-03 13:46:32'),
(16, 'กองทุนหมุนเวียนส่งเสริมผลผลิต', 2, 1, '2020-11-03 13:46:37'),
(17, 'รายได้แผ่นดิน', 2, 1, '2020-11-03 13:46:51'),
(19, 'ปัจจัย  พฐ นร ยากจน มัธยม', 2, 1, '2020-11-03 13:47:11'),
(20, 'ปัจจัย  พฐ นร ยากจน ประถม', 2, 1, '2020-11-03 13:47:21'),
(21, 'กสศ', 3, 1, '2020-11-03 13:47:31'),
(22, 'รายได้สถานศึกษา', 2, 1, '2020-11-03 13:47:41');

-- --------------------------------------------------------

--
-- Table structure for table `educationarea`
--

CREATE TABLE `educationarea` (
  `Id` int(11) NOT NULL,
  `Name` varchar(255) COLLATE utf8mb4_general_ci NOT NULL,
  `CreatedDate` datetime NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `educationarea`
--

INSERT INTO `educationarea` (`Id`, `Name`, `CreatedDate`) VALUES
(1, 'เขต 1', '2020-12-07 12:44:53');

-- --------------------------------------------------------

--
-- Table structure for table `partner`
--

CREATE TABLE `partner` (
  `Id` int(11) NOT NULL,
  `Name` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `VatNumber` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `Address` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `PartnerType` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `IsInternal` tinyint(1) NOT NULL,
  `CreatedDate` datetime NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `partner`
--

INSERT INTO `partner` (`Id`, `Name`, `VatNumber`, `Address`, `PartnerType`, `IsInternal`, `CreatedDate`) VALUES
(1, 'นายสุริยัน เกกาฤทธิ์', NULL, NULL, 'Normal', 1, '2020-11-24 16:28:48'),
(2, 'นายกฤษฎา จันทร์ประเสริฐ', NULL, NULL, 'Normal', 1, '2020-11-25 14:51:17'),
(3, 'การไฟฟ้าส่วนภูมิภาค', NULL, NULL, 'Normal', 0, '2020-11-25 14:51:33'),
(4, 'นางสาววิภาพร แซ่ว่าง', '8-5715-8412881-1', '19 หมู่ 3 ตำบลแม่จัน อำเภอแม่จัน จังหวัดเชียงราย 57110', 'Person', 0, '2020-11-25 14:51:59'),
(5, 'นางสาวลลิตา แซ่ฟู่', '8571584130823', '567/4 ม.1 ต.เทอดไทย อ.แม่ฟ้าหลวง จ.เชียงราย', 'Person', 0, '2020-11-25 14:52:16'),
(6, 'นายจตุรงค์ เข็ดขาม', '1510600054938', '85 หมู่ 8 ต.แม่คำ อ.แม่จัน จ.เชียงราย ', 'Person', 0, '2020-11-25 14:52:41'),
(7, 'กรมสรรพากร', NULL, NULL, 'Normal', 0, '2020-11-25 14:53:16'),
(8, 'นางสุขใจ สนิทรักษ์', '3509900744578', '127/4 หมู่ 13 ต.รอบเวียง อ.เมืองเชียงราย จ.เชียงราย 570000', 'Person', 0, '2020-11-25 14:53:36'),
(9, 'นางสาวจารุรัตน์  วัฒนทวีวงค์', '8571584003325', '16 ม.6 ต.เทอดไทย อ.แม่ฟ้าหลวง จ.เชียงราย ', 'Person', 0, '2020-11-25 14:53:56'),
(10, 'นางพัชรพร โลหิตกาญจน์', '3570900041684', '444/1 หมู่ 3 ต.แม่จัน อ.แม่จัน จ.เชียงราย 57110', 'Person', 0, '2020-11-25 14:54:21'),
(11, 'นายดวงทิตย์ มัฆวาฬ', '3570101029039', '154 หมู่ 18 ต.ห้วยสัก อ.เมืองเชียงราย จ.เชียงราย', 'Person', 0, '2020-11-25 14:54:44'),
(12, 'บริษัท ทรู วิชั่นส์ กรุ๊ป จำกัด', '0105551056821', '118/1 อาคารทิปโก้ ถนนพระรวม 6 แขวงพญาไท เขตพญาไท กทม. 10400', 'Shop', 0, '2020-11-25 14:55:24'),
(13, 'บริษัท มิวนิคบุ๊คเซ็นเตอร์ จำกัด', '0575541000322', '630 ถนนสิงหไคล ตำบลเวียง อำเภอเมืองเชียงราย จังหวัดเชียงราย 57000', 'Person', 0, '2020-11-25 14:55:49'),
(14, 'นางสาวปัทมา กองล้าน', '3660700164013', '40/1 หมู่ 8 ต.วังโมกข์ อ.วชิรบารมี จ.พิจิตร', 'Person', 0, '2020-11-25 14:56:10'),
(15, 'หจก.เชียงรายเทคโนโปร', '0573539001277', '139/1-2 ม.9 ต.รอบเวียง อ.เมือง จ.เชียงราย 57000', 'Shop', 0, '2020-11-25 14:56:26'),
(16, 'หจก.ไอที โอเอ แอนด์ เซอร์วิส', '0573563001443', '567/4 ม.1 ต.เทอดไทย อ.แม่ฟ้าหลวง จ.เชียงราย', 'Shop', 0, '2020-11-25 14:57:01'),
(17, 'นายพานิตร์ นันตา', '3500900338989', '123 หมู่ 14 ต.สันทราย อ.ฝาง จ.เชียงใหม่ 50110', 'Person', 0, '2020-11-25 14:57:20'),
(18, 'บริษัท ทีโอที จำกัด (มหาชน)', '0107545000161', '89/2 ถนนแจ้งวัฒนะ แขวงทุ่งสองห้อง เขตหลักสี่ กรุงเทพ 10210', 'Shop', 0, '2020-11-25 14:57:42'),
(19, 'เงินอุดหนุนค่าอาหารสำหรับนักเรียนพักนอนโรงเรียนบ้านพญาไพร', NULL, NULL, 'Normal', 0, '2020-11-25 14:58:01'),
(20, 'นายพิชิต ปัญญาวิชัย', '1570700051116', '179 หมู่ 7 ต.แม่จัน อ.แม่จัน จ.เชียงราย 57110', 'Person', 0, '2020-11-25 14:58:22'),
(21, 'บริษัท ทรู อินเตอร์เน็ต คอร์ปอเรชั่น จำกัด', '0105549025026', '18 อาคารทรูทาวเวอร์ ถนนรัชดาภิเษก แขวงห้วยขวาง เขตห้วยขวาง กทม 10310', 'Shop', 0, '2020-11-25 14:59:22'),
(22, 'นายวีระยุทธ เลิศไกรวัล', '3579900247680', '222 หมู่ 17 ตำบลรอบเวียง อำเภอเมืองเชียงราย จังหวัดเชียงราย 57000', 'Person', 0, '2020-11-25 14:59:52');

-- --------------------------------------------------------

--
-- Table structure for table `school`
--

CREATE TABLE `school` (
  `Id` int(11) NOT NULL,
  `Name` varchar(255) COLLATE utf8mb4_general_ci NOT NULL,
  `Address` varchar(255) COLLATE utf8mb4_general_ci NOT NULL,
  `VatId` varchar(255) COLLATE utf8mb4_general_ci NOT NULL,
  `EducationAreaId` int(11) NOT NULL,
  `CreatedDate` datetime NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `school`
--

INSERT INTO `school` (`Id`, `Name`, `Address`, `VatId`, `EducationAreaId`, `CreatedDate`) VALUES
(1, 'โรงเรียนบ้านพญาไพร', '111 หมู่ที่ 6 บ้านพญาไพรเล่าจอ ต.เทอดไทย อ.แม่ฟ้าหลวง จ.เชียงราย', '994000675178', 1, '2020-12-07 14:12:17');

-- --------------------------------------------------------

--
-- Table structure for table `schoolyear`
--

CREATE TABLE `schoolyear` (
  `Id` int(11) NOT NULL,
  `Year` varchar(4) COLLATE utf8mb4_general_ci NOT NULL,
  `StartDate` date NOT NULL,
  `SchoolId` int(11) NOT NULL,
  `CreatedDate` datetime NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `schoolyear`
--

INSERT INTO `schoolyear` (`Id`, `Year`, `StartDate`, `SchoolId`, `CreatedDate`) VALUES
(1, '2563', '2019-01-10', 1, '2020-12-12 07:46:46');

-- --------------------------------------------------------

--
-- Table structure for table `transaction`
--

CREATE TABLE `transaction` (
  `Id` int(11) NOT NULL,
  `IssueDate` datetime NOT NULL,
  `DuplicatePaymentType` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `DuplicatePaymentNumber` varchar(255) COLLATE utf8mb4_general_ci DEFAULT NULL,
  `DuplicatePaymentCount` int(11) DEFAULT NULL,
  `DuplicatePaymentYear` varchar(255) COLLATE utf8mb4_general_ci DEFAULT NULL,
  `Title` varchar(255) COLLATE utf8mb4_general_ci NOT NULL,
  `Remark` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `PartnerId` int(11) DEFAULT NULL,
  `Amount` decimal(10,2) NOT NULL,
  `PaymentType` varchar(255) COLLATE utf8mb4_general_ci DEFAULT NULL,
  `VatInclude` decimal(10,2) DEFAULT NULL,
  `ProductType` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `BudgetId` int(11) NOT NULL,
  `SchoolId` int(11) NOT NULL,
  `CreatedDate` datetime NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `transaction`
--

INSERT INTO `transaction` (`Id`, `IssueDate`, `DuplicatePaymentType`, `DuplicatePaymentNumber`, `DuplicatePaymentCount`, `DuplicatePaymentYear`, `Title`, `Remark`, `PartnerId`, `Amount`, `PaymentType`, `VatInclude`, `ProductType`, `BudgetId`, `SchoolId`, `CreatedDate`) VALUES
(20, '2019-10-01 00:00:00', 'ไม่ระบุ', NULL, NULL, NULL, 'ยอดยกมา', NULL, NULL, '30820.91', NULL, NULL, '', 7, 1, '2020-11-09 15:44:11'),
(21, '2019-10-09 00:00:00', 'บค.', '1', 1, '63', 'คืนเงินคงเหลือจากงบประมาณปี 2562', NULL, NULL, '-29720.00', 'DuplicatePayment', NULL, '', 7, 1, '2020-11-09 15:46:30'),
(22, '2019-11-01 00:00:00', 'บร.', '17ก63220/2', 1, NULL, 'รับเงินอุดหนุนค่าอาหารกลางวันนักเรียน จำนวน 445 คน ไตรมาสที่ 1', NULL, NULL, '445000.00', NULL, NULL, '', 7, 1, '2020-11-09 15:49:11'),
(23, '2019-11-01 22:49:00', 'บค.', '3', 2, '63', 'จ่ายค่าอาหารกลางวันนักเรียน 6 วัน', NULL, NULL, '-53400.00', 'Debtor', NULL, '', 7, 1, '2020-11-09 15:50:02'),
(24, '2019-11-07 22:50:00', 'บค.', '8', 2, '63', 'จ่ายค่าอาหารกลางวันนักเรียน 10 วัน', NULL, NULL, '-89000.00', 'DuplicatePayment', NULL, '', 7, 1, '2020-11-09 15:51:25'),
(25, '2019-11-22 22:58:00', 'บค.', '16', 2, '63', 'จ่ายค่าอาหารกลางวันนักเรียน 9 วัน', NULL, NULL, '-80100.00', 'DuplicatePayment', NULL, '', 7, 1, '2020-11-09 15:58:55'),
(26, '2019-12-06 00:00:00', 'บค.', '24', 1, '63', 'จ่ายค่าอาหารกลางวันนักเรียน 9 วัน', NULL, NULL, '-80100.00', 'DuplicatePayment', NULL, '', 7, 1, '2020-11-09 15:59:43'),
(27, '2019-12-20 00:00:00', 'บค.', '30', 2, '63', 'จ่ายค่าอาหารกลางวันนักเรียน 7 วัน', NULL, NULL, '-62300.00', 'DuplicatePayment', NULL, '', 7, 1, '2020-11-09 16:00:18'),
(28, '2020-01-03 00:00:00', 'บค.', '35', 1, '63', 'จ่ายค่าอาหารกลางวันนักเรียน 9 วัน', NULL, NULL, '-80100.00', 'DuplicatePayment', NULL, '', 7, 1, '2020-11-10 15:49:26'),
(29, '2020-01-17 22:50:00', '', NULL, NULL, NULL, 'รับเงินอุดหนุนค่าอาหารกลางวันไตรมาส 2', NULL, NULL, '445000.00', NULL, NULL, '', 7, 1, '2020-11-10 15:50:19'),
(30, '2020-01-17 00:00:00', 'บค.', '43', 2, '63', 'จ่ายอาหารกลางวัน 10 วัน', NULL, NULL, '-89000.00', 'DuplicatePayment', NULL, '', 7, 1, '2020-11-10 15:53:20'),
(31, '2020-01-31 00:00:00', 'บค.', '52', 2, '63', 'จ่ายค่าอาหารกลางวันนักเรียน 9 วัน', NULL, NULL, '-80100.00', 'DuplicatePayment', NULL, '', 7, 1, '2020-11-10 15:53:43'),
(35, '2019-10-01 00:00:00', 'ไม่ระบุ', NULL, NULL, NULL, 'ยอดยกมา', NULL, NULL, '20103.13', NULL, NULL, '', 8, 1, '2020-11-28 16:19:41'),
(36, '2019-10-25 00:00:00', '', NULL, NULL, NULL, 'รับเงินอุดหนุนค่าจ้างบุคลากร เจ้าหน้าที่ทรูปลูกปัญญา', NULL, NULL, '46200.00', NULL, NULL, '', 8, 1, '2020-11-28 16:20:28'),
(37, '2019-10-28 00:00:00', 'บค.', '2', 1, '63', 'จ่ายเงินเดือนเจ้าหน้าที่ทรูปลูกปัญญา เดือน ตุลาคม 2562', NULL, 1, '-15400.00', 'DuplicatePayment', NULL, '', 8, 1, '2020-11-28 16:21:47'),
(38, '2019-11-07 00:00:00', '', NULL, NULL, NULL, 'รับเงินอุดหนุน 70% 2/2562 ค่าจัดการเรียนการสอน', NULL, NULL, '426600.00', NULL, NULL, '', 8, 1, '2020-11-28 17:11:46'),
(39, '2019-11-06 00:00:00', NULL, NULL, NULL, NULL, 'คืนเงินยืมประเภท กิจกรรมพัฒนาผู้เรียน', NULL, NULL, '-100000.00', 'DuplicatePayment', NULL, '', 8, 1, '2020-11-28 17:16:12'),
(40, '2019-11-07 00:00:00', 'บค.', '5', 1, '63', 'เงินยืมค่าอาหารนักเรียนพักนอน 50 วัน 2/2562', NULL, 2, '-56000.00', 'Debtor', NULL, '', 8, 1, '2020-11-28 17:17:02'),
(41, '2019-11-15 00:00:00', 'บค.', '10', 1, '63', 'ค่าเดินทางไปราชการ การแข่งขันทักษะวิชาการ เขต14,880.- ,ครูภานุมาศ 3,568 ,ครูสุริยัน 3,012', NULL, 2, '-21460.00', 'DuplicatePayment', NULL, '', 8, 1, '2020-11-28 17:17:42'),
(42, '2019-11-15 00:00:00', 'บจ.', '12', 1, '63', 'ค่าไฟฟ้า เดือน ตุลาคม 2562', NULL, 3, '-12633.78', 'DuplicatePayment', NULL, '', 8, 1, '2020-11-28 17:18:30'),
(43, '2019-11-21 00:00:00', '', NULL, NULL, NULL, 'รับเงินอุดหนุนค่าอาหารนักเรียนพักนอน 2/2562', NULL, NULL, '112000.00', NULL, NULL, '', 8, 1, '2020-11-28 17:18:57'),
(44, '2019-11-29 00:00:00', 'บค.', '18', 1, '63', 'เงินเดือนเจ้าหน้าที่ทรูปลูกปัญญา เดือนพฤศจิกายน 2562', NULL, 2, '-15400.00', 'DuplicatePayment', NULL, '', 8, 1, '2020-11-28 17:19:33'),
(45, '2019-12-06 00:00:00', 'บค.', '19', 1, '63', 'ค่าเดินทางไปราชการ ครูกฤษฎา 7,464 และครูพรรณิภา 1,772', NULL, 2, '-9236.00', 'DuplicatePayment', NULL, '', 8, 1, '2020-11-30 12:04:08'),
(46, '2019-12-06 00:00:00', 'บจ.', '20', 1, '63', 'โครงการ พัฒนาภาษาไทยสำหรับเด็กที่ไม่ใช้ภาษาไทยเป็นภาษาแม่ 10,000 ,วันลอยกระทง 8,000 ,ส่งเสริมความเป็นเลิศทางวิชาการ 25,000', NULL, 4, '-43000.00', 'DuplicatePayment', '430.00', 'วัสดุ', 8, 1, '2020-11-30 12:05:33'),
(47, '2019-12-06 00:00:00', 'บจ.', '21', 1, '63', 'พัฒนาระบบประชาสัมพันธ์ 13,000 ,พัฒนาอัจฉริยภาพทางด้านเทคโนโลยี 25,000', NULL, 5, '-38000.00', 'DuplicatePayment', '380.00', 'วัสดุ', 8, 1, '2020-11-30 12:08:59'),
(48, '2019-12-06 00:00:00', 'บจ.', '22', 1, '63', 'ค่าไฟฟ้า เดือน พฤศจิกายน 2562', NULL, 3, '-13237.31', 'DuplicatePayment', NULL, '', 8, 1, '2020-11-30 12:10:04'),
(49, '2019-12-13 00:00:00', 'บค.', '27', 1, '63', 'เงินยืมโครงการทักษะวิชาการระดับชาติ', NULL, 2, '-20160.00', 'Debtor', NULL, '', 8, 1, '2020-11-30 12:11:29'),
(50, '2019-12-20 00:00:00', 'บจ.', '28', 1, '63', 'โครงการนักประวัติศาสตร์น้อย 2,730 ,บ้านนักวิทย์น้อย 5,000 บาท', NULL, 4, '-7730.00', 'DuplicatePayment', '77.30', 'วัสดุ', 8, 1, '2020-11-30 12:12:38'),
(51, '2019-12-23 00:00:00', 'บจ.', '27', 1, '63', 'โครงการส่งเสริมความเป็นเลิศทางวิชาการ(ทักษะวิชาการระดับชาติ รายการจ้างเหมารถ)', NULL, 6, '-27000.00', 'DuplicatePayment', '270.00', 'จ้างเหมารถ', 8, 1, '2020-11-30 12:13:22'),
(52, '2019-12-23 00:00:00', 'บค.', '33', 1, '63', 'เงินยืมค่าอาหารนักเรียนกีฬากลุ่มเทอดไทย', NULL, 2, '-10000.00', 'DuplicatePayment', '0.00', 'อาหาร', 8, 1, '2020-11-30 12:14:07'),
(53, '2019-12-26 00:00:00', '', NULL, NULL, NULL, 'รับเงินอุดหนุนรายหัว 30 % ค่าจัดการเรียนการสอน', NULL, NULL, '181450.00', NULL, NULL, '', 8, 1, '2020-11-30 13:00:23'),
(54, '2019-12-27 00:00:00', 'บค.', '34', 1, '63', 'เงินเดือนเจ้าหน้าที่ทรูปลูกปัญญา เดือนธันวาคม 2562', NULL, 2, '-15400.00', 'DuplicatePayment', NULL, '', 8, 1, '2020-11-30 13:01:12'),
(57, '2020-01-08 00:00:00', 'ไม่ระบุ', NULL, NULL, NULL, 'ภาษี ณ ที่จ่ายเดือน ธันวาคม 2562', NULL, NULL, '-1157.30', NULL, NULL, '', 0, 1, '2020-12-06 14:29:04'),
(61, '2021-01-11 00:00:00', 'บจ.', '1', 2, '2563', 'ยกยอดascasdasd', NULL, 19, '-10000.00', 'ใบสำคัญ', '0.00', NULL, 7, 1, '2021-01-11 15:34:54'),
(64, '2021-01-11 00:00:00', 'บจ.', '3', 4, '2563', 'ยกยอดascasdasd', NULL, 7, '-10000.00', 'ใบสำคัญ', '0.00', NULL, 7, 1, '2021-01-11 15:39:45');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `bankaccount`
--
ALTER TABLE `bankaccount`
  ADD PRIMARY KEY (`Id`);

--
-- Indexes for table `bringforward`
--
ALTER TABLE `bringforward`
  ADD PRIMARY KEY (`Id`);

--
-- Indexes for table `budget`
--
ALTER TABLE `budget`
  ADD PRIMARY KEY (`Id`);

--
-- Indexes for table `educationarea`
--
ALTER TABLE `educationarea`
  ADD PRIMARY KEY (`Id`);

--
-- Indexes for table `partner`
--
ALTER TABLE `partner`
  ADD PRIMARY KEY (`Id`);

--
-- Indexes for table `school`
--
ALTER TABLE `school`
  ADD PRIMARY KEY (`Id`);

--
-- Indexes for table `schoolyear`
--
ALTER TABLE `schoolyear`
  ADD PRIMARY KEY (`Id`);

--
-- Indexes for table `transaction`
--
ALTER TABLE `transaction`
  ADD PRIMARY KEY (`Id`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `bankaccount`
--
ALTER TABLE `bankaccount`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT for table `bringforward`
--
ALTER TABLE `bringforward`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=12219;

--
-- AUTO_INCREMENT for table `budget`
--
ALTER TABLE `budget`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=24;

--
-- AUTO_INCREMENT for table `educationarea`
--
ALTER TABLE `educationarea`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT for table `partner`
--
ALTER TABLE `partner`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=23;

--
-- AUTO_INCREMENT for table `school`
--
ALTER TABLE `school`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT for table `schoolyear`
--
ALTER TABLE `schoolyear`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT for table `transaction`
--
ALTER TABLE `transaction`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=65;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
