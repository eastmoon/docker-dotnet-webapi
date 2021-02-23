-- migrate:up
ALTER TABLE users ADD title varchar(255);
ALTER TABLE users ADD nickname varchar(255);

-- migrate:down
ALTER TABLE users DROP COLUMN title;
ALTER TABLE users DROP COLUMN nickname;
