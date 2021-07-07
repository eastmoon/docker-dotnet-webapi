-- migrate:up
ALTER TABLE users ADD title varchar(255) DEFAULT NULL;
ALTER TABLE users ADD nickname varchar(255) DEFAULT NULL;

-- migrate:down
ALTER TABLE users DROP COLUMN title;
ALTER TABLE users DROP COLUMN nickname;
