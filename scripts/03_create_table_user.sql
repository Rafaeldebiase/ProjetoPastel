create table pastel.tb_user(
  id uuid primary key,
  first_name text not null,
  last_name text not null,
  birth_date date not null,
  email text not null,
  pass text not null,
  street text not null,
  street_number int not null,
  street_complement text null,
  neighborhood text not null,
  city text not null,
  state text not null,
  country text not null,
  zip_code text not null,
  user_role text not null,
  manager_id uuid not null
);