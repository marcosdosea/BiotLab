-- Atualizacao de schema para experimentos com multiplos pesquisadores,
-- cepa opcional, titulo padronizado e origens do harem.
-- Execute no banco do BiotLab antes de usar as novas telas.

ALTER TABLE experimento
    ADD COLUMN titulo VARCHAR(100) NULL AFTER id;

UPDATE experimento
SET titulo = COALESCE(NULLIF(titulo, ''), NULLIF(tituloProjeto, ''), CONCAT('Experimento ', id));

ALTER TABLE experimento
    MODIFY COLUMN titulo VARCHAR(100) NOT NULL,
    MODIFY COLUMN cepa VARCHAR(50) NULL;

CREATE TABLE experimentoPesquisador (
    idExperimento INT UNSIGNED NOT NULL,
    idPesquisador INT UNSIGNED NOT NULL,
    PRIMARY KEY (idExperimento, idPesquisador),
    INDEX fk_ExperimentoPesquisador_Pesquisador1_idx (idPesquisador),
    CONSTRAINT fk_ExperimentoPesquisador_Experimento1
        FOREIGN KEY (idExperimento)
        REFERENCES experimento (id)
        ON DELETE CASCADE
        ON UPDATE NO ACTION,
    CONSTRAINT fk_ExperimentoPesquisador_Pesquisador1
        FOREIGN KEY (idPesquisador)
        REFERENCES pesquisador (id)
        ON DELETE RESTRICT
        ON UPDATE NO ACTION
);

INSERT IGNORE INTO experimentoPesquisador (idExperimento, idPesquisador)
SELECT id, idPesquisador
FROM experimento
WHERE idPesquisador IS NOT NULL;

ALTER TABLE harem
    ADD COLUMN origemPai VARCHAR(100) NOT NULL DEFAULT 'Nao informado' AFTER dataNascimento,
    ADD COLUMN origemMae VARCHAR(100) NOT NULL DEFAULT 'Nao informado' AFTER origemPai;
