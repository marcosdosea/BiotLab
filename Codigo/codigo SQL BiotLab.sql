-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';

-- -----------------------------------------------------
-- Schema mydb
-- -----------------------------------------------------

-- -----------------------------------------------------
-- Schema mydb
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `mydb` DEFAULT CHARACTER SET utf8 ;
USE `mydb` ;

-- -----------------------------------------------------
-- Table `mydb`.`TB_Instituição`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `mydb`.`TB_Instituição` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `Nome` VARCHAR(255) NOT NULL,
  `Cnpj` CHAR(14) NOT NULL,
  `Telefone` VARCHAR(20) NULL,
  `Email` VARCHAR(255) NOT NULL,
  `Endereço` VARCHAR(255) NOT NULL,
  `UF` CHAR(2) NOT NULL,
  `CEP` CHAR(8) NOT NULL,
  PRIMARY KEY (`Id`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `mydb`.`TB_Fornecedor`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `mydb`.`TB_Fornecedor` (
  `id` INT NOT NULL AUTO_INCREMENT,
  PRIMARY KEY (`id`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `mydb`.`TB_Pesquisador`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `mydb`.`TB_Pesquisador` (
  `ID` INT NOT NULL AUTO_INCREMENT,
  `Nome` VARCHAR(255) NOT NULL,
  `CPF` CHAR(11) NOT NULL,
  `Endereço` VARCHAR(255) NOT NULL,
  `Instituição` VARCHAR(255) NOT NULL,
  PRIMARY KEY (`ID`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `mydb`.`TB_Bioterio`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `mydb`.`TB_Bioterio` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `NumeroGaiola` INT NOT NULL,
  `Cidade` VARCHAR(100) NOT NULL,
  `Responsavel` VARCHAR(255) NOT NULL,
  `NomeDoBioterio` VARCHAR(255) NOT NULL,
  `TB_Pesquisador_ID` INT NOT NULL,
  `TB_Instituição_Id` INT NOT NULL,
  PRIMARY KEY (`Id`),
  INDEX `fk_TB_Bioterio_TB_Pesquisador1_idx` (`TB_Pesquisador_ID` ASC) VISIBLE,
  INDEX `fk_TB_Bioterio_TB_Instituição1_idx` (`TB_Instituição_Id` ASC) VISIBLE,
  CONSTRAINT `fk_TB_Bioterio_TB_Pesquisador1`
    FOREIGN KEY (`TB_Pesquisador_ID`)
    REFERENCES `mydb`.`TB_Pesquisador` (`ID`)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT,
  CONSTRAINT `fk_TB_Bioterio_TB_Instituição1`
    FOREIGN KEY (`TB_Instituição_Id`)
    REFERENCES `mydb`.`TB_Instituição` (`Id`)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `mydb`.`TB_Entrada`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `mydb`.`TB_Entrada` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `NumeroNotaFiscal` VARCHAR(20) NOT NULL,
  `DataEntrada` DATE NOT NULL,
  `TB_Fornecedor_id` INT NOT NULL,
  `TB_Bioterio_Id` INT NOT NULL,
  `TB_Pesquisador_ID1` INT NOT NULL,
  PRIMARY KEY (`Id`),
  INDEX `fk_TB_Entrada_TB_Fornecedor1_idx` (`TB_Fornecedor_id` ASC) VISIBLE,
  INDEX `fk_TB_Entrada_TB_Bioterio1_idx` (`TB_Bioterio_Id` ASC) VISIBLE,
  INDEX `fk_TB_Entrada_TB_Pesquisador2_idx` (`TB_Pesquisador_ID1` ASC) VISIBLE,
  CONSTRAINT `fk_TB_Entrada_TB_Fornecedor1`
    FOREIGN KEY (`TB_Fornecedor_id`)
    REFERENCES `mydb`.`TB_Fornecedor` (`id`)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT,
  CONSTRAINT `fk_TB_Entrada_TB_Bioterio1`
    FOREIGN KEY (`TB_Bioterio_Id`)
    REFERENCES `mydb`.`TB_Bioterio` (`Id`)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT,
  CONSTRAINT `fk_TB_Entrada_TB_Pesquisador2`
    FOREIGN KEY (`TB_Pesquisador_ID1`)
    REFERENCES `mydb`.`TB_Pesquisador` (`ID`)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `mydb`.`TB_Anestesicos`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `mydb`.`TB_Anestesicos` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `Nome` VARCHAR(255) NOT NULL,
  `Dosagem` VARCHAR(50) NOT NULL,
  `Marca` VARCHAR(100) NOT NULL,
  `Anestesicoscol` VARCHAR(45) NOT NULL,
  `TB_Instituição_Id` INT NOT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_TB_Anestesicos_TB_Instituição1_idx` (`TB_Instituição_Id` ASC) VISIBLE,
  CONSTRAINT `fk_TB_Anestesicos_TB_Instituição1`
    FOREIGN KEY (`TB_Instituição_Id`)
    REFERENCES `mydb`.`TB_Instituição` (`Id`)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `mydb`.`TB_Haren`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `mydb`.`TB_Haren` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `Origem` VARCHAR(255) NOT NULL,
  `Natalidade` DATE NOT NULL,
  PRIMARY KEY (`Id`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `mydb`.`TB_Gaiola`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `mydb`.`TB_Gaiola` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `NumControleInterno` VARCHAR(50) NOT NULL,
  `Bioterio` VARCHAR(255) NOT NULL,
  `Localização` VARCHAR(255) NOT NULL,
  `TB_Bioterio_Id` INT NOT NULL,
  PRIMARY KEY (`Id`),
  INDEX `fk_TB_Gaiola_TB_Bioterio1_idx` (`TB_Bioterio_Id` ASC) VISIBLE,
  CONSTRAINT `fk_TB_Gaiola_TB_Bioterio1`
    FOREIGN KEY (`TB_Bioterio_Id`)
    REFERENCES `mydb`.`TB_Bioterio` (`Id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `mydb`.`TB_Experimento`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `mydb`.`TB_Experimento` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `DataInicio` DATE NOT NULL,
  `PesquisadorResponsavel` CHAR(11) NOT NULL,
  `DataFIm` DATE NOT NULL,
  `Gaiola` INT NOT NULL,
  `Cepa` VARCHAR(50) NOT NULL,
  `Experimentocol` VARCHAR(45) NOT NULL,
  `TB_Pesquisador_ID` INT NOT NULL,
  PRIMARY KEY (`Id`),
  INDEX `fk_TB_Experimento_TB_Pesquisador1_idx` (`TB_Pesquisador_ID` ASC) VISIBLE,
  CONSTRAINT `fk_TB_Experimento_TB_Pesquisador1`
    FOREIGN KEY (`TB_Pesquisador_ID`)
    REFERENCES `mydb`.`TB_Pesquisador` (`ID`)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `mydb`.`TB_EntradaAnestesico`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `mydb`.`TB_EntradaAnestesico` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `Quantidade` INT NOT NULL,
  `Lote` VARCHAR(50) NOT NULL,
  `ValorUnitario` DECIMAL(10,2) NOT NULL,
  `Subtotal` DECIMAL(10,2) NOT NULL,
  PRIMARY KEY (`Id`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `mydb`.`TB_UsoAnestesicos`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `mydb`.`TB_UsoAnestesicos` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `NomeAnestesicos` VARCHAR(255) NOT NULL,
  `Volume` DECIMAL(10,2) NOT NULL,
  `Data` DATE NOT NULL,
  `Procedimento` VARCHAR(255) NOT NULL,
  `Cepa` VARCHAR(50) NOT NULL,
  `AnimaisEnvolvidos` INT NOT NULL,
  `TB_Experimento_Id` INT NOT NULL,
  `TB_Pesquisador_ID` INT NOT NULL,
  `TB_EntradaAnestesico_Id` INT NOT NULL,
  PRIMARY KEY (`Id`),
  INDEX `fk_TB_UsoAnestesicos_TB_Experimento1_idx` (`TB_Experimento_Id` ASC) VISIBLE,
  INDEX `fk_TB_UsoAnestesicos_TB_Pesquisador1_idx` (`TB_Pesquisador_ID` ASC) VISIBLE,
  INDEX `fk_TB_UsoAnestesicos_TB_EntradaAnestesico1_idx` (`TB_EntradaAnestesico_Id` ASC) VISIBLE,
  CONSTRAINT `fk_TB_UsoAnestesicos_TB_Experimento1`
    FOREIGN KEY (`TB_Experimento_Id`)
    REFERENCES `mydb`.`TB_Experimento` (`Id`)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT,
  CONSTRAINT `fk_TB_UsoAnestesicos_TB_Pesquisador1`
    FOREIGN KEY (`TB_Pesquisador_ID`)
    REFERENCES `mydb`.`TB_Pesquisador` (`ID`)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT,
  CONSTRAINT `fk_TB_UsoAnestesicos_TB_EntradaAnestesico1`
    FOREIGN KEY (`TB_EntradaAnestesico_Id`)
    REFERENCES `mydb`.`TB_EntradaAnestesico` (`Id`)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `mydb`.`TB_PovoarGaiola`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `mydb`.`TB_PovoarGaiola` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `EstadoDaGaiola` VARCHAR(50) NOT NULL,
  `Sexo` CHAR(1) NOT NULL,
  `Peso` DECIMAL(10,2) NOT NULL,
  `QuantidadeDeAnimais` INT NOT NULL,
  `HarenOrigem` VARCHAR(255) NOT NULL,
  `BioterioOrigem` VARCHAR(255) NOT NULL,
  `DataNascimento` DATE NOT NULL,
  `DataChegada` DATE NOT NULL,
  `TB_Pesquisador_ID` INT NOT NULL,
  `TB_Haren_Id` INT NOT NULL,
  `TB_Gaiola_Id` INT NOT NULL,
  PRIMARY KEY (`Id`),
  INDEX `fk_TB_PovoarGaiola_TB_Pesquisador1_idx` (`TB_Pesquisador_ID` ASC) VISIBLE,
  INDEX `fk_TB_PovoarGaiola_TB_Haren1_idx` (`TB_Haren_Id` ASC) VISIBLE,
  INDEX `fk_TB_PovoarGaiola_TB_Gaiola1_idx` (`TB_Gaiola_Id` ASC) VISIBLE,
  CONSTRAINT `fk_TB_PovoarGaiola_TB_Pesquisador1`
    FOREIGN KEY (`TB_Pesquisador_ID`)
    REFERENCES `mydb`.`TB_Pesquisador` (`ID`)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT,
  CONSTRAINT `fk_TB_PovoarGaiola_TB_Haren1`
    FOREIGN KEY (`TB_Haren_Id`)
    REFERENCES `mydb`.`TB_Haren` (`Id`)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT,
  CONSTRAINT `fk_TB_PovoarGaiola_TB_Gaiola1`
    FOREIGN KEY (`TB_Gaiola_Id`)
    REFERENCES `mydb`.`TB_Gaiola` (`Id`)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `mydb`.`TB_Obituario`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `mydb`.`TB_Obituario` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `DataObito` DATE NOT NULL,
  `GaiolaOrigem` INT NOT NULL,
  `TB_Pesquisador_ID` INT NOT NULL,
  `TB_Gaiola_Id` INT NOT NULL,
  PRIMARY KEY (`Id`),
  INDEX `fk_TB_Obituario_TB_Pesquisador1_idx` (`TB_Pesquisador_ID` ASC) VISIBLE,
  INDEX `fk_TB_Obituario_TB_Gaiola1_idx` (`TB_Gaiola_Id` ASC) VISIBLE,
  CONSTRAINT `fk_TB_Obituario_TB_Pesquisador1`
    FOREIGN KEY (`TB_Pesquisador_ID`)
    REFERENCES `mydb`.`TB_Pesquisador` (`ID`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_TB_Obituario_TB_Gaiola1`
    FOREIGN KEY (`TB_Gaiola_Id`)
    REFERENCES `mydb`.`TB_Gaiola` (`Id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `mydb`.`TB_Anestesicos_has_TB_Entrada`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `mydb`.`TB_Anestesicos_has_TB_Entrada` (
  `TB_Anestesicos_id` INT NOT NULL,
  `TB_Entrada_Id` INT NOT NULL,
  `TB_EntradaAnestesico_Id` INT NOT NULL,
  PRIMARY KEY (`TB_Anestesicos_id`, `TB_Entrada_Id`, `TB_EntradaAnestesico_Id`),
  INDEX `fk_TB_Anestesicos_has_TB_Entrada_TB_Entrada1_idx` (`TB_Entrada_Id` ASC) VISIBLE,
  INDEX `fk_TB_Anestesicos_has_TB_Entrada_TB_Anestesicos_idx` (`TB_Anestesicos_id` ASC) VISIBLE,
  INDEX `fk_TB_Anestesicos_has_TB_Entrada_TB_EntradaAnestesico1_idx` (`TB_EntradaAnestesico_Id` ASC) VISIBLE,
  CONSTRAINT `fk_TB_Anestesicos_has_TB_Entrada_TB_Anestesicos`
    FOREIGN KEY (`TB_Anestesicos_id`)
    REFERENCES `mydb`.`TB_Anestesicos` (`id`)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT,
  CONSTRAINT `fk_TB_Anestesicos_has_TB_Entrada_TB_Entrada1`
    FOREIGN KEY (`TB_Entrada_Id`)
    REFERENCES `mydb`.`TB_Entrada` (`Id`)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT,
  CONSTRAINT `fk_TB_Anestesicos_has_TB_Entrada_TB_EntradaAnestesico1`
    FOREIGN KEY (`TB_EntradaAnestesico_Id`)
    REFERENCES `mydb`.`TB_EntradaAnestesico` (`Id`)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `mydb`.`TB_Gaiola_has_TB_Haren`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `mydb`.`TB_Gaiola_has_TB_Haren` (
  `TB_Gaiola_Id` INT NOT NULL,
  `TB_Haren_Id` INT NOT NULL,
  PRIMARY KEY (`TB_Gaiola_Id`, `TB_Haren_Id`),
  INDEX `fk_TB_Gaiola_has_TB_Haren_TB_Haren1_idx` (`TB_Haren_Id` ASC) VISIBLE,
  INDEX `fk_TB_Gaiola_has_TB_Haren_TB_Gaiola1_idx` (`TB_Gaiola_Id` ASC) VISIBLE,
  CONSTRAINT `fk_TB_Gaiola_has_TB_Haren_TB_Gaiola1`
    FOREIGN KEY (`TB_Gaiola_Id`)
    REFERENCES `mydb`.`TB_Gaiola` (`Id`)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT,
  CONSTRAINT `fk_TB_Gaiola_has_TB_Haren_TB_Haren1`
    FOREIGN KEY (`TB_Haren_Id`)
    REFERENCES `mydb`.`TB_Haren` (`Id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `mydb`.`TB_Gaiola_has_TB_Experimento`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `mydb`.`TB_Gaiola_has_TB_Experimento` (
  `TB_Gaiola_Id` INT NOT NULL,
  `TB_Experimento_Id` INT NOT NULL,
  PRIMARY KEY (`TB_Gaiola_Id`, `TB_Experimento_Id`),
  INDEX `fk_TB_Gaiola_has_TB_Experimento_TB_Experimento1_idx` (`TB_Experimento_Id` ASC) VISIBLE,
  INDEX `fk_TB_Gaiola_has_TB_Experimento_TB_Gaiola1_idx` (`TB_Gaiola_Id` ASC) VISIBLE,
  CONSTRAINT `fk_TB_Gaiola_has_TB_Experimento_TB_Gaiola1`
    FOREIGN KEY (`TB_Gaiola_Id`)
    REFERENCES `mydb`.`TB_Gaiola` (`Id`)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT,
  CONSTRAINT `fk_TB_Gaiola_has_TB_Experimento_TB_Experimento1`
    FOREIGN KEY (`TB_Experimento_Id`)
    REFERENCES `mydb`.`TB_Experimento` (`Id`)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT)
ENGINE = InnoDB;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
