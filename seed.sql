CREATE TABLE IF NOT EXISTS public.todoitems
(
    id uuid NOT NULL,
    title text COLLATE pg_catalog."default",
    "isCompleted" boolean NOT NULL,
    CONSTRAINT "todoItems_pkey" PRIMARY KEY (id)
);

CREATE EXTENSION "uuid-ossp";