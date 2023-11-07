
CREATE TABLE item_categories(
    category_id uuid PRIMARY KEY NOT NULL,
    name text NOT NULL
);

CREATE TABLE items(
    item_id uuid PRIMARY KEY NOT NULL,
    name text NOT NULL,
    category_id uuid REFERENCES item_categories(category_id) NOT NULL
);

CREATE INDEX items_by_category_id ON items(category_id);

CREATE TABLE shopping_lists(
    shopping_list_id uuid PRIMARY KEY NOT NULL,
    name text NOT NULL,
    created_on timestamp NOT NULL,
    last_updated timestamp NOT NULL
);

CREATE TABLE shopping_list_items(
    shopping_list_item_id uuid PRIMARY KEY NOT NULL,
    shopping_list_id uuid NOT NULL REFERENCES shopping_lists(shopping_list_id),
    item_id uuid NOT NULL REFERENCES items(item_id),
    amount bigint NOT NULL,
    added_on timestamp NOT NULL
);

CREATE INDEX shopping_list_items_by_shopping_list_id ON shopping_list_items(shopping_list_item_id);
