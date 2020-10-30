-- MySQL dump 10.13  Distrib 8.0.18, for Win64 (x86_64)
--
-- Host: localhost    Database: dbinventaris
-- ------------------------------------------------------
-- Server version	8.0.18

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Dumping data for table `tblapparaat`
--

LOCK TABLES `tblapparaat` WRITE;
/*!40000 ALTER TABLE `tblapparaat` DISABLE KEYS */;
INSERT INTO `tblapparaat` VALUES (91,42,'123',NULL,1,'AF/12',NULL,NULL,0);
INSERT INTO `tblapparaat` VALUES (92,43,'127',NULL,2,'AF/15',NULL,NULL,0);
/*!40000 ALTER TABLE `tblapparaat` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `tblhistoriek`
--

LOCK TABLES `tblhistoriek` WRITE;
/*!40000 ALTER TABLE `tblhistoriek` DISABLE KEYS */;
INSERT INTO `tblhistoriek` VALUES (24,'2020-01-14',NULL,17,91);
INSERT INTO `tblhistoriek` VALUES (25,'2020-01-16',NULL,18,92);
/*!40000 ALTER TABLE `tblhistoriek` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `tblleveranciers`
--

LOCK TABLES `tblleveranciers` WRITE;
/*!40000 ALTER TABLE `tblleveranciers` DISABLE KEYS */;
INSERT INTO `tblleveranciers` VALUES (1,'Bol.com','Scofield','michael.scofield@gmail.com','0489965896');
INSERT INTO `tblleveranciers` VALUES (2,'Coolblue.com','Bellick','Bellick.officer@gmail.com','048812541');
/*!40000 ALTER TABLE `tblleveranciers` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `tbllocatie`
--

LOCK TABLES `tbllocatie` WRITE;
/*!40000 ALTER TABLE `tbllocatie` DISABLE KEYS */;
INSERT INTO `tbllocatie` VALUES (11,'Kantoor','Test');
INSERT INTO `tbllocatie` VALUES (12,'Toilet','');
/*!40000 ALTER TABLE `tbllocatie` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `tblmerk`
--

LOCK TABLES `tblmerk` WRITE;
/*!40000 ALTER TABLE `tblmerk` DISABLE KEYS */;
INSERT INTO `tblmerk` VALUES (28,'Logitech'),(29,'Dell');
INSERT INTO `tblmerk` VALUES (30,'Corsair'),(31,'HP');
/*!40000 ALTER TABLE `tblmerk` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `tblmodel`
--

LOCK TABLES `tblmodel` WRITE;
/*!40000 ALTER TABLE `tblmodel` DISABLE KEYS */;
INSERT INTO `tblmodel` VALUES (41,'MK250',28,28),(42,'MK260',29,29);
INSERT INTO `tblmodel` VALUES (43,'SUGAR',30,30),(44,'BANAN',30,30);
/*!40000 ALTER TABLE `tblmodel` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `tbltype`
--

LOCK TABLES `tbltype` WRITE;
/*!40000 ALTER TABLE `tbltype` DISABLE KEYS */;
INSERT INTO `tbltype` VALUES (28,'Toetsenbord'),(29,'Muis');
INSERT INTO `tbltype` VALUES (30,'Router'),(31,'Modem');
/*!40000 ALTER TABLE `tbltype` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `tblwerknemer`
--

LOCK TABLES `tblwerknemer` WRITE;
/*!40000 ALTER TABLE `tblwerknemer` DISABLE KEYS */;
INSERT INTO `tblwerknemer` VALUES (17,'Alexie Chaerle',11,'','Test');
INSERT INTO `tblwerknemer` VALUES (18,'John',12,'','');
/*!40000 ALTER TABLE `tblwerknemer` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2020-01-14  8:55:14
