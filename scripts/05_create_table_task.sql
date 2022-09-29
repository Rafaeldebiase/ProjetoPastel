create table pastel.tb_task
(
    id uuid primary key,
    user_id uuid not null,
    message text not null,
    deadline date not null,
    completed bool not null
)