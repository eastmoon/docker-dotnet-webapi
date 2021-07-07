-- migrate:up
create table users (
  id integer NOT NULL AUTO_INCREMENT,
  create_time timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  update_time timestamp NULL DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  name varchar(255) DEFAULT "abc",
  email varchar(255) DEFAULT NULL,
  PRIMARY KEY (`id`)
);

-- migrate:down
drop table users;
