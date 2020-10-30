-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';

-- -----------------------------------------------------
-- Schema mydb
-- -----------------------------------------------------
-- -----------------------------------------------------
-- Schema dbinventaris
-- -----------------------------------------------------
DROP SCHEMA IF EXISTS `dbinventaris` ;

-- -----------------------------------------------------
-- Schema dbinventaris
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `dbinventaris` DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci ;
USE `dbinventaris` ;

-- -----------------------------------------------------
-- Table `dbinventaris`.`tblmerk`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `dbinventaris`.`tblmerk` ;

CREATE TABLE IF NOT EXISTS `dbinventaris`.`tblmerk` (
  `id` INT(11) NOT NULL AUTO_INCREMENT,
  `omschrijving` VARCHAR(45) NULL DEFAULT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB
AUTO_INCREMENT = 28
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `dbinventaris`.`tbltype`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `dbinventaris`.`tbltype` ;

CREATE TABLE IF NOT EXISTS `dbinventaris`.`tbltype` (
  `id` INT(11) NOT NULL AUTO_INCREMENT,
  `omschrijving` VARCHAR(45) NULL DEFAULT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB
AUTO_INCREMENT = 28
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `dbinventaris`.`tblmodel`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `dbinventaris`.`tblmodel` ;

CREATE TABLE IF NOT EXISTS `dbinventaris`.`tblmodel` (
  `id` INT(11) NOT NULL AUTO_INCREMENT,
  `omschrijving` VARCHAR(45) NULL DEFAULT NULL,
  `typeID` INT(11) NOT NULL,
  `merkID` INT(11) NOT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_tblmodel_tbltype_idx` (`typeID` ASC) VISIBLE,
  INDEX `fk_tblmodel_tblmerk1_idx` (`merkID` ASC) VISIBLE,
  CONSTRAINT `fk_tblmodel_tblmerk1`
    FOREIGN KEY (`merkID`)
    REFERENCES `dbinventaris`.`tblmerk` (`id`),
  CONSTRAINT `fk_tblmodel_tbltype`
    FOREIGN KEY (`typeID`)
    REFERENCES `dbinventaris`.`tbltype` (`id`))
ENGINE = InnoDB
AUTO_INCREMENT = 41
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `dbinventaris`.`tblleveranciers`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `dbinventaris`.`tblleveranciers` ;

CREATE TABLE IF NOT EXISTS `dbinventaris`.`tblleveranciers` (
  `id` INT(11) NOT NULL AUTO_INCREMENT,
  `naam` VARCHAR(45) NULL DEFAULT NULL,
  `contactpersoon` VARCHAR(45) NULL DEFAULT NULL,
  `email` VARCHAR(45) NULL DEFAULT NULL,
  `telefoonnummer` VARCHAR(45) NULL DEFAULT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `dbinventaris`.`tblapparaat`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `dbinventaris`.`tblapparaat` ;

CREATE TABLE IF NOT EXISTS `dbinventaris`.`tblapparaat` (
  `id` INT(11) NOT NULL AUTO_INCREMENT,
  `modelID` INT(11) NULL DEFAULT NULL,
  `serienummer` VARCHAR(45) NULL DEFAULT NULL,
  `uitgebruik` INT(11) NULL DEFAULT NULL,
  `leverancierID` INT NULL DEFAULT NULL,
  `factuurnummer` VARCHAR(45) NULL DEFAULT NULL,
  `aankoopdatum` VARCHAR(45) NULL DEFAULT NULL,
  `commentaar` VARCHAR(45) NULL DEFAULT NULL,
  `stock` TINYINT(1) NULL DEFAULT 0,
  PRIMARY KEY (`id`),
  INDEX `fk_tblapparaat_tblmodel1_idx` (`modelID` ASC) VISIBLE,
  INDEX `fk_tblapparaat_tblleveranciers1_idx` (`leverancierID` ASC) VISIBLE,
  CONSTRAINT `fk_tblapparaat_tblmodel1`
    FOREIGN KEY (`modelID`)
    REFERENCES `dbinventaris`.`tblmodel` (`id`),
  CONSTRAINT `fk_tblapparaat_tblleveranciers1`
    FOREIGN KEY (`leverancierID`)
    REFERENCES `dbinventaris`.`tblleveranciers` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
AUTO_INCREMENT = 86
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `dbinventaris`.`tbllocatie`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `dbinventaris`.`tbllocatie` ;

CREATE TABLE IF NOT EXISTS `dbinventaris`.`tbllocatie` (
  `id` INT(11) NOT NULL AUTO_INCREMENT,
  `omschrijving` VARCHAR(10) NULL DEFAULT NULL,
  `commentaar` VARCHAR(45) NULL DEFAULT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB
AUTO_INCREMENT = 11
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `dbinventaris`.`tblwerknemer`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `dbinventaris`.`tblwerknemer` ;

CREATE TABLE IF NOT EXISTS `dbinventaris`.`tblwerknemer` (
  `id` INT(11) NOT NULL AUTO_INCREMENT,
  `naam` VARCHAR(45) NULL DEFAULT NULL,
  `locatieID` INT(11) NULL DEFAULT NULL,
  `status` VARCHAR(45) NULL DEFAULT NULL,
  `commentaar` VARCHAR(45) NULL DEFAULT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_tblwerknemer_tbllocatie1_idx` (`locatieID` ASC) VISIBLE,
  CONSTRAINT `fk_tblwerknemer_tbllocatie1`
    FOREIGN KEY (`locatieID`)
    REFERENCES `dbinventaris`.`tbllocatie` (`id`))
ENGINE = InnoDB
AUTO_INCREMENT = 15
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `dbinventaris`.`tblhistoriek`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `dbinventaris`.`tblhistoriek` ;

CREATE TABLE IF NOT EXISTS `dbinventaris`.`tblhistoriek` (
  `id` INT(11) NOT NULL AUTO_INCREMENT,
  `ingebruik` VARCHAR(45) NULL DEFAULT NULL,
  `uitgebruik` VARCHAR(45) NULL DEFAULT NULL,
  `werknemerID` INT(11) NOT NULL,
  `apparaatID` INT(11) NOT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_tblhistoriek_tblwerknemer1_idx` (`werknemerID` ASC) VISIBLE,
  INDEX `fk_tblhistoriek_tblapparaat1_idx` (`apparaatID` ASC) VISIBLE,
  CONSTRAINT `fk_tblhistoriek_tblapparaat1`
    FOREIGN KEY (`apparaatID`)
    REFERENCES `dbinventaris`.`tblapparaat` (`id`),
  CONSTRAINT `fk_tblhistoriek_tblwerknemer1`
    FOREIGN KEY (`werknemerID`)
    REFERENCES `dbinventaris`.`tblwerknemer` (`id`))
ENGINE = InnoDB
AUTO_INCREMENT = 20
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
