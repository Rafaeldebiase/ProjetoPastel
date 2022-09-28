create table pastel.tb_image
(
    id uuid primary key,
    image_name text not null,
    image_data bytea not null,
    content_type text not null,
    user_id uuid not null
)