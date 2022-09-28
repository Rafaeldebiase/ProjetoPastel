create table pastel.tb_phone
(
    id uuid primary key,
    user_id uuid not null,
    phone_number text not null,
    phone_type text not null
)