-- migrate:up
create table temp (
  id integer,
  name varchar(255),
  email varchar(255) not null,
  var1 varchar(255) not null,
  var2 varchar(255) not null
);

-- migrate:down
drop table temp;
