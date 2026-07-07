ALTER TABLE experimento
ADD COLUMN tituloProjeto VARCHAR(150) NOT NULL DEFAULT 'Projeto sem titulo' AFTER id;
