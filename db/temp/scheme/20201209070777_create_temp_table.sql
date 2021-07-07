-- migrate:up
create table temp (
  id integer NOT NULL AUTO_INCREMENT,
  create_time timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  update_time timestamp NULL DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  name varchar(255) DEFAULT "abc",
  email varchar(255) DEFAULT NULL,
  var1 varchar(255) DEFAULT NULL,
  var2 varchar(255) DEFAULT NULL,
  PRIMARY KEY (`id`)
);

-- migrate:down
drop table temp;
