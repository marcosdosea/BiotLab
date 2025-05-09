-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';

-- -----------------------------------------------------
-- Schema BiotLab
-- -----------------------------------------------------

-- -----------------------------------------------------
-- Schema BiotLab
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `BiotLab` DEFAULT CHARACTER SET utf8 ;
USE `BiotLab` ;

-- -----------------------------------------------------
-- Table `BiotLab`.`Pesquisador`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `BiotLab`.`Pesquisador` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `cpf` VARCHAR(11) NOT NULL,
  `nome` VARCHAR(50) NOT NULL,
  `cep` VARCHAR(9) NOT NULL,
  `rua` VARCHAR(50) NULL,
  `bairro` VARCHAR(50) NULL,
  `cidade` VARCHAR(50) NULL,
  `numero` VARCHAR(20) NULL,
  `complemento` VARCHAR(50) NULL,
  `estado` VARCHAR(2) NOT NULL,
  `telefone1` VARCHAR(15) NOT NULL,
  `telefone2` VARCHAR(15) NULL,
  `email` VARCHAR(50) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE INDEX `cpf_UNIQUE` (`cpf` ASC) VISIBLE)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `BiotLab`.`Experimento`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `BiotLab`.`Experimento` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `dataInicio` DATE NOT NULL,
  `dataFim` DATE NOT NULL,
  `cepa` VARCHAR(50) NOT NULL,
  `idPesquisador` INT UNSIGNED NOT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_Experimento_Pesquisador1_idx` (`idPesquisador` ASC) VISIBLE,
  CONSTRAINT `fk_Experimento_Pesquisador1`
    FOREIGN KEY (`idPesquisador`)
    REFERENCES `BiotLab`.`Pesquisador` (`id`)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `BiotLab`.`Instituicao`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `BiotLab`.`Instituicao` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `nome` VARCHAR(50) NOT NULL,
  `cnpj` VARCHAR(18) NOT NULL,
  `cep` VARCHAR(9) NOT NULL,
  `rua` VARCHAR(50) NULL,
  `bairro` VARCHAR(50) NULL,
  `cidade` VARCHAR(50) NULL,
  `numero` VARCHAR(20) NULL,
  `complemento` VARCHAR(50) NULL,
  `estado` VARCHAR(2) NOT NULL,
  `telefone1` VARCHAR(15) NOT NULL,
  `telefone2` VARCHAR(15) NULL,
  `email` VARCHAR(50) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE INDEX `cnpj_UNIQUE` (`cnpj` ASC) VISIBLE)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `BiotLab`.`Anestesico`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `BiotLab`.`Anestesico` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `nome` VARCHAR(50) NOT NULL,
  `marca` VARCHAR(50) NOT NULL,
  `concentracao` DECIMAL(10,2) NOT NULL,
  `idInstituicao` INT UNSIGNED NOT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_Anestesico_Instituicao1_idx` (`idInstituicao` ASC) VISIBLE,
  CONSTRAINT `fk_Anestesico_Instituicao1`
    FOREIGN KEY (`idInstituicao`)
    REFERENCES `BiotLab`.`Instituicao` (`id`)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `BiotLab`.`Fornecedor`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `BiotLab`.`Fornecedor` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `nome` VARCHAR(50) NOT NULL,
  `cnpj` VARCHAR(18) NOT NULL,
  `cep` VARCHAR(9) NOT NULL,
  `rua` VARCHAR(50) NULL,
  `bairro` VARCHAR(50) NULL,
  `cidade` VARCHAR(50) NULL,
  `numero` VARCHAR(20) NULL,
  `complemento` VARCHAR(50) NULL,
  `estado` VARCHAR(2) NOT NULL,
  `telefone1` VARCHAR(15) NOT NULL,
  `telefone2` VARCHAR(15) NULL,
  `email` VARCHAR(50) NOT NULL,
  `idInstituicao` INT UNSIGNED NOT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_Fornecedor_Instituicao1_idx` (`idInstituicao` ASC) VISIBLE,
  CONSTRAINT `fk_Fornecedor_Instituicao1`
    FOREIGN KEY (`idInstituicao`)
    REFERENCES `BiotLab`.`Instituicao` (`id`)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `BiotLab`.`Entrada`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `BiotLab`.`Entrada` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `numeroNotaFiscal` VARCHAR(20) NOT NULL,
  `dataEntrada` DATETIME NOT NULL,
  `idFornecedor` INT UNSIGNED NOT NULL,
  `idInstituicao` INT UNSIGNED NOT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_Entrada_Fornecedor1_idx` (`idFornecedor` ASC) VISIBLE,
  INDEX `fk_Entrada_Instituicao1_idx` (`idInstituicao` ASC) VISIBLE,
  CONSTRAINT `fk_Entrada_Fornecedor1`
    FOREIGN KEY (`idFornecedor`)
    REFERENCES `BiotLab`.`Fornecedor` (`id`)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT,
  CONSTRAINT `fk_Entrada_Instituicao1`
    FOREIGN KEY (`idInstituicao`)
    REFERENCES `BiotLab`.`Instituicao` (`id`)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `BiotLab`.`EntradaAnestesico`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `BiotLab`.`EntradaAnestesico` (
  `idEntrada` INT UNSIGNED NOT NULL,
  `idAnestesico` INT UNSIGNED NOT NULL,
  `quantidade` DECIMAL(10,2) NOT NULL,
  `lote` VARCHAR(50) NOT NULL,
  `valorUnitario` DECIMAL(10,2) NOT NULL,
  `subTotal` DECIMAL(10,2) NOT NULL,
  PRIMARY KEY (`idEntrada`, `idAnestesico`),
  INDEX `fk_EntradaAnestesico_Entrada1_idx` (`idEntrada` ASC) VISIBLE,
  CONSTRAINT `fk_EntradaAnestesico_Anestesico1`
    FOREIGN KEY (`idAnestesico`)
    REFERENCES `BiotLab`.`Anestesico` (`id`)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT,
  CONSTRAINT `fk_EntradaAnestesico_Entrada1`
    FOREIGN KEY (`idEntrada`)
    REFERENCES `BiotLab`.`Entrada` (`id`)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `BiotLab`.`UsoAnestesico`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `BiotLab`.`UsoAnestesico` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `quantidade` DECIMAL(10,2) NOT NULL,
  `procedimento` VARCHAR(255) NOT NULL,
  `data` DATETIME NOT NULL,
  `cepa` VARCHAR(50) NOT NULL,
  `numeroAnimais` INT NOT NULL,
  `idPesquisador` INT UNSIGNED NOT NULL,
  `idExperimento` INT UNSIGNED NOT NULL,
  `idEntrada` INT UNSIGNED NOT NULL,
  `idAnestesico` INT UNSIGNED NOT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_usoAnestesico_Pesquisador1_idx` (`idPesquisador` ASC) VISIBLE,
  INDEX `fk_usoAnestesico_Experimento1_idx` (`idExperimento` ASC) VISIBLE,
  INDEX `fk_usoAnestesico_EntradaAnestesico1_idx` (`idEntrada` ASC, `idAnestesico` ASC) VISIBLE,
  CONSTRAINT `fk_usoAnestesico_Pesquisador1`
    FOREIGN KEY (`idPesquisador`)
    REFERENCES `BiotLab`.`Pesquisador` (`id`)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT,
  CONSTRAINT `fk_usoAnestesico_Experimento1`
    FOREIGN KEY (`idExperimento`)
    REFERENCES `BiotLab`.`Experimento` (`id`)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT,
  CONSTRAINT `fk_usoAnestesico_EntradaAnestesico1`
    FOREIGN KEY (`idEntrada` , `idAnestesico`)
    REFERENCES `BiotLab`.`EntradaAnestesico` (`idEntrada` , `idAnestesico`)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `BiotLab`.`Bioterio`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `BiotLab`.`Bioterio` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `nome` VARCHAR(50) NOT NULL,
  `cep` VARCHAR(9) NOT NULL,
  `rua` VARCHAR(50) NULL,
  `bairro` VARCHAR(50) NULL,
  `cidade` VARCHAR(50) NULL,
  `numero` VARCHAR(20) NULL,
  `complemento` VARCHAR(50) NULL,
  `estado` VARCHAR(2) NOT NULL,
  `telefone1` VARCHAR(15) NOT NULL,
  `telefone2` VARCHAR(15) NULL,
  `email` VARCHAR(50) NOT NULL,
  `idInstituicao` INT UNSIGNED NOT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_Bioterio_Instituicao1_idx` (`idInstituicao` ASC) VISIBLE,
  CONSTRAINT `fk_Bioterio_Instituicao1`
    FOREIGN KEY (`idInstituicao`)
    REFERENCES `BiotLab`.`Instituicao` (`id`)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `BiotLab`.`Gaiola`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `BiotLab`.`Gaiola` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `codigoInterno` VARCHAR(50) NOT NULL,
  `numeroMachos` INT NOT NULL,
  `numeroFemeas` INT NOT NULL,
  `etiqueta` VARCHAR(50) NULL,
  `localizacao` VARCHAR(100) NOT NULL,
  `status` ENUM('N', 'E', 'F') NOT NULL COMMENT 'N - NOVA\nE - EXPERIMENTO SENDO REALIZADO\nF - EXPERIMENTO FINALIZADO',
  `idBioterio` INT UNSIGNED NOT NULL,
  `idExperimento` INT UNSIGNED NOT NULL,
  `idPesquisador` INT UNSIGNED NOT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_Gaiola_Bioterio1_idx` (`idBioterio` ASC) VISIBLE,
  INDEX `fk_Gaiola_experimento1_idx` (`idExperimento` ASC) VISIBLE,
  INDEX `fk_Gaiola_Pesquisador1_idx` (`idPesquisador` ASC) VISIBLE,
  CONSTRAINT `fk_Gaiola_Bioterio1`
    FOREIGN KEY (`idBioterio`)
    REFERENCES `BiotLab`.`Bioterio` (`id`)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT,
  CONSTRAINT `fk_Gaiola_experimento1`
    FOREIGN KEY (`idExperimento`)
    REFERENCES `BiotLab`.`Experimento` (`id`)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT,
  CONSTRAINT `fk_Gaiola_Pesquisador1`
    FOREIGN KEY (`idPesquisador`)
    REFERENCES `BiotLab`.`Pesquisador` (`id`)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `BiotLab`.`Harem`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `BiotLab`.`Harem` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `codigoInterno` VARCHAR(20) NOT NULL,
  `numeroMachos` INT NOT NULL,
  `numeroFemeas` INT NOT NULL,
  `dataNascimento` DATETIME NOT NULL,
  `status` ENUM('A', 'I') NOT NULL COMMENT 'A - ATIVO\nI - INATIVO',
  `idBioterio` INT UNSIGNED NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE INDEX `codigoInterno_UNIQUE` (`codigoInterno` ASC) VISIBLE,
  INDEX `fk_Harem_Bioterio1_idx` (`idBioterio` ASC) VISIBLE,
  CONSTRAINT `fk_Harem_Bioterio1`
    FOREIGN KEY (`idBioterio`)
    REFERENCES `BiotLab`.`Bioterio` (`id`)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `BiotLab`.`obituario`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `BiotLab`.`obituario` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `data` DATETIME NOT NULL,
  `idGaiola` INT UNSIGNED NOT NULL,
  `idPesquisador` INT UNSIGNED NOT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_obituario_Gaiola1_idx` (`idGaiola` ASC) VISIBLE,
  INDEX `fk_obituario_Pesquisador1_idx` (`idPesquisador` ASC) VISIBLE,
  CONSTRAINT `fk_obituario_Gaiola1`
    FOREIGN KEY (`idGaiola`)
    REFERENCES `BiotLab`.`Gaiola` (`id`)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT,
  CONSTRAINT `fk_obituario_Pesquisador1`
    FOREIGN KEY (`idPesquisador`)
    REFERENCES `BiotLab`.`Pesquisador` (`id`)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `BiotLab`.`GaiolaHarem`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `BiotLab`.`GaiolaHarem` (
  `idGaiola` INT UNSIGNED NOT NULL,
  `idHarem` INT UNSIGNED NOT NULL,
  `dataPovoamento` DATETIME NOT NULL,
  `idPesquisador` INT UNSIGNED NOT NULL,
  PRIMARY KEY (`idGaiola`, `idHarem`),
  INDEX `fk_GaiolaHarem_Harem1_idx` (`idHarem` ASC) VISIBLE,
  INDEX `fk_GaiolaHarem_Gaiola1_idx` (`idGaiola` ASC) VISIBLE,
  INDEX `fk_GaiolaHarem_Pesquisador1_idx` (`idPesquisador` ASC) VISIBLE,
  CONSTRAINT `fk_GaiolaHarem_Gaiola1`
    FOREIGN KEY (`idGaiola`)
    REFERENCES `BiotLab`.`Gaiola` (`id`)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT,
  CONSTRAINT `fk_GaiolaHarem_Harem1`
    FOREIGN KEY (`idHarem`)
    REFERENCES `BiotLab`.`Harem` (`id`)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT,
  CONSTRAINT `fk_GaiolaHarem_Pesquisador1`
    FOREIGN KEY (`idPesquisador`)
    REFERENCES `BiotLab`.`Pesquisador` (`id`)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT)
ENGINE = InnoDB;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
