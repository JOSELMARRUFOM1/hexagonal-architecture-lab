CREATE TABLE public.budgets (
    id UUID PRIMARY KEY,
    name TEXT NOT NULL CHECK (length(btrim(name)) > 0),
    amount NUMERIC NOT NULL CHECK (amount > 0),
    created_at TIMESTAMPTZ NOT NULL
);
