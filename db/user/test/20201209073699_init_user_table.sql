-- migrate:up
INSERT INTO users (id, name, email)
VALUES (1, "test-name", "test@mail.com");

-- migrate:down
DELETE FROM users WHERE id=1;
