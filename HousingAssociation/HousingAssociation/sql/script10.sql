﻿CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    migration_id character varying(150) NOT NULL,
    product_version character varying(32) NOT NULL,
    CONSTRAINT pk___ef_migrations_history PRIMARY KEY (migration_id)
);

START TRANSACTION;

CREATE TABLE addresses (
    id integer GENERATED BY DEFAULT AS IDENTITY,
    city character varying(255) NOT NULL,
    district character varying(255) NULL,
    street character varying(255) NOT NULL,
    CONSTRAINT pk_addresses PRIMARY KEY (id)
);

CREATE TABLE users (
    id integer GENERATED BY DEFAULT AS IDENTITY,
    first_name character varying(255) NOT NULL,
    last_name character varying(255) NOT NULL,
    phone_number character varying(10) NOT NULL,
    email character varying(255) NOT NULL,
    role text NOT NULL,
    is_enabled boolean NOT NULL,
    CONSTRAINT pk_users PRIMARY KEY (id),
    CONSTRAINT "CK_users_role_Enum" CHECK (role IN ('Admin', 'Worker', 'Resident'))
);

CREATE TABLE buildings (
    id integer GENERATED BY DEFAULT AS IDENTITY,
    number character varying(255) NOT NULL,
    address_id integer NOT NULL,
    type text NOT NULL,
    CONSTRAINT pk_buildings PRIMARY KEY (id),
    CONSTRAINT "CK_buildings_type_Enum" CHECK (type IN ('Block', 'House')),
    CONSTRAINT fk_buildings_addresses_address_id FOREIGN KEY (address_id) REFERENCES addresses (id) ON DELETE CASCADE
);

CREATE TABLE announcements (
    id integer GENERATED BY DEFAULT AS IDENTITY,
    type text NOT NULL,
    expiration_date timestamp without time zone NULL,
    is_cancelled_or_expired boolean NOT NULL,
    previous_announcement_id integer NULL,
    title character varying(255) NULL,
    content character varying(255) NULL,
    author_id integer NOT NULL,
    created_at timestamp without time zone NOT NULL,
    CONSTRAINT pk_announcements PRIMARY KEY (id),
    CONSTRAINT "CK_announcements_type_Enum" CHECK (type IN ('Issue', 'Announcement', 'Alert')),
    CONSTRAINT fk_announcements_users_author_id FOREIGN KEY (author_id) REFERENCES users (id) ON DELETE CASCADE
);

CREATE TABLE credentials (
    user_id integer NOT NULL,
    password_hash character varying(255) NOT NULL,
    CONSTRAINT pk_credentials PRIMARY KEY (user_id),
    CONSTRAINT fk_credentials_users_user_id FOREIGN KEY (user_id) REFERENCES users (id) ON DELETE CASCADE
);

CREATE TABLE documents (
    id integer GENERATED BY DEFAULT AS IDENTITY,
    title text NULL,
    author_id integer NOT NULL,
    created_at timestamp without time zone NOT NULL,
    filepath text NOT NULL,
    md5 text NOT NULL,
    days_to_expire integer NULL,
    CONSTRAINT pk_documents PRIMARY KEY (id),
    CONSTRAINT fk_documents_users_author_id FOREIGN KEY (author_id) REFERENCES users (id) ON DELETE CASCADE
);

CREATE TABLE refresh_tokens (
    id integer GENERATED BY DEFAULT AS IDENTITY,
    user_id integer NOT NULL,
    token text NULL,
    expires timestamp without time zone NOT NULL,
    created timestamp without time zone NOT NULL,
    revoked timestamp without time zone NULL,
    replaced_by_token text NULL,
    reason_revoked text NULL,
    CONSTRAINT pk_refresh_tokens PRIMARY KEY (id),
    CONSTRAINT fk_refresh_tokens_users_user_id FOREIGN KEY (user_id) REFERENCES users (id) ON DELETE CASCADE
);

CREATE TABLE locals (
    id integer GENERATED BY DEFAULT AS IDENTITY,
    number integer NULL,
    area real NULL,
    building_id integer NOT NULL,
    is_fully_owned boolean NOT NULL,
    CONSTRAINT pk_locals PRIMARY KEY (id),
    CONSTRAINT fk_locals_buildings_building_id FOREIGN KEY (building_id) REFERENCES buildings (id) ON DELETE CASCADE
);

CREATE TABLE announcements_buildings (
    announcements_id integer NOT NULL,
    target_buildings_id integer NOT NULL,
    CONSTRAINT pk_announcements_buildings PRIMARY KEY (announcements_id, target_buildings_id),
    CONSTRAINT fk_announcements_buildings_announcements_announcements_id FOREIGN KEY (announcements_id) REFERENCES announcements (id) ON DELETE CASCADE,
    CONSTRAINT fk_announcements_buildings_buildings_target_buildings_id FOREIGN KEY (target_buildings_id) REFERENCES buildings (id) ON DELETE CASCADE
);

CREATE TABLE users_documents (
    documents_id integer NOT NULL,
    receivers_id integer NOT NULL,
    CONSTRAINT pk_users_documents PRIMARY KEY (documents_id, receivers_id),
    CONSTRAINT fk_users_documents_documents_documents_id FOREIGN KEY (documents_id) REFERENCES documents (id) ON DELETE CASCADE,
    CONSTRAINT fk_users_documents_users_receivers_id FOREIGN KEY (receivers_id) REFERENCES users (id) ON DELETE CASCADE
);

CREATE TABLE issues (
    id integer GENERATED BY DEFAULT AS IDENTITY,
    source_local_id integer NULL,
    source_building_id integer NOT NULL,
    is_resolved boolean NOT NULL,
    is_cancelled boolean NOT NULL,
    title character varying(255) NULL,
    content character varying(255) NULL,
    author_id integer NOT NULL,
    created_at timestamp without time zone NOT NULL,
    CONSTRAINT pk_issues PRIMARY KEY (id),
    CONSTRAINT fk_issues_buildings_source_building_id FOREIGN KEY (source_building_id) REFERENCES buildings (id) ON DELETE CASCADE,
    CONSTRAINT fk_issues_locals_source_local_id FOREIGN KEY (source_local_id) REFERENCES locals (id) ON DELETE RESTRICT,
    CONSTRAINT fk_issues_users_author_id FOREIGN KEY (author_id) REFERENCES users (id) ON DELETE CASCADE
);

CREATE TABLE locals_owners (
    owned_locals_id integer NOT NULL,
    owners_id integer NOT NULL,
    CONSTRAINT pk_locals_owners PRIMARY KEY (owned_locals_id, owners_id),
    CONSTRAINT fk_locals_owners_locals_owned_locals_id FOREIGN KEY (owned_locals_id) REFERENCES locals (id) ON DELETE CASCADE,
    CONSTRAINT fk_locals_owners_users_owners_id FOREIGN KEY (owners_id) REFERENCES users (id) ON DELETE CASCADE
);

CREATE TABLE locals_residents (
    resided_locals_id integer NOT NULL,
    residents_id integer NOT NULL,
    CONSTRAINT pk_locals_residents PRIMARY KEY (resided_locals_id, residents_id),
    CONSTRAINT fk_locals_residents_locals_resided_locals_id FOREIGN KEY (resided_locals_id) REFERENCES locals (id) ON DELETE CASCADE,
    CONSTRAINT fk_locals_residents_users_residents_id FOREIGN KEY (residents_id) REFERENCES users (id) ON DELETE CASCADE
);

CREATE INDEX ix_announcements_author_id ON announcements (author_id);

CREATE INDEX ix_announcements_buildings_target_buildings_id ON announcements_buildings (target_buildings_id);

CREATE INDEX ix_buildings_address_id ON buildings (address_id);

CREATE INDEX ix_documents_author_id ON documents (author_id);

CREATE INDEX ix_issues_author_id ON issues (author_id);

CREATE INDEX ix_issues_source_building_id ON issues (source_building_id);

CREATE INDEX ix_issues_source_local_id ON issues (source_local_id);

CREATE INDEX ix_locals_building_id ON locals (building_id);

CREATE INDEX ix_locals_owners_owners_id ON locals_owners (owners_id);

CREATE INDEX ix_locals_residents_residents_id ON locals_residents (residents_id);

CREATE INDEX ix_refresh_tokens_user_id ON refresh_tokens (user_id);

CREATE INDEX ix_users_documents_receivers_id ON users_documents (receivers_id);

INSERT INTO "__EFMigrationsHistory" (migration_id, product_version)
VALUES ('20211214001918_InitialMigration', '5.0.11');

COMMIT;

START TRANSACTION;

ALTER TABLE issues DROP COLUMN created_at;

ALTER TABLE issues DROP COLUMN is_cancelled;

ALTER TABLE issues DROP COLUMN is_resolved;

ALTER TABLE documents DROP COLUMN created_at;

ALTER TABLE announcements DROP COLUMN created_at;

ALTER TABLE announcements DROP COLUMN is_cancelled_or_expired;

ALTER TABLE refresh_tokens ALTER COLUMN revoked TYPE timestamp with time zone;

ALTER TABLE refresh_tokens ALTER COLUMN expires TYPE timestamp with time zone;

ALTER TABLE refresh_tokens ALTER COLUMN created TYPE timestamp with time zone;

ALTER TABLE issues ADD cancelled timestamp with time zone NULL;

ALTER TABLE issues ADD created timestamp with time zone NOT NULL DEFAULT TIMESTAMPTZ '0001-01-01 00:00:00+00:00';

ALTER TABLE issues ADD previous_issue_id integer NULL;

ALTER TABLE issues ADD resolved timestamp with time zone NULL;

ALTER TABLE documents ADD created timestamp with time zone NOT NULL DEFAULT TIMESTAMPTZ '0001-01-01 00:00:00+00:00';

ALTER TABLE announcements ALTER COLUMN expiration_date TYPE timestamp with time zone;

ALTER TABLE announcements ADD cancelled timestamp with time zone NULL;

ALTER TABLE announcements ADD created timestamp with time zone NOT NULL DEFAULT TIMESTAMPTZ '0001-01-01 00:00:00+00:00';

ALTER TABLE announcements ADD expired timestamp with time zone NULL;

CREATE INDEX ix_issues_previous_issue_id ON issues (previous_issue_id);

CREATE INDEX ix_announcements_previous_announcement_id ON announcements (previous_announcement_id);

ALTER TABLE announcements ADD CONSTRAINT fk_announcements_announcements_previous_announcement_id FOREIGN KEY (previous_announcement_id) REFERENCES announcements (id) ON DELETE RESTRICT;

ALTER TABLE issues ADD CONSTRAINT fk_issues_issues_previous_issue_id FOREIGN KEY (previous_issue_id) REFERENCES issues (id) ON DELETE RESTRICT;

INSERT INTO "__EFMigrationsHistory" (migration_id, product_version)
VALUES ('20211214100600_ModifyEvents', '5.0.11');

COMMIT;

START TRANSACTION;

INSERT INTO "__EFMigrationsHistory" (migration_id, product_version)
VALUES ('20211214154844_AddBuildingToLocal', '5.0.11');

COMMIT;

START TRANSACTION;

ALTER TABLE locals ALTER COLUMN number TYPE text;

INSERT INTO "__EFMigrationsHistory" (migration_id, product_version)
VALUES ('20211214155427_ChangeLocalNumberTypeToString', '5.0.11');

COMMIT;

START TRANSACTION;

INSERT INTO "__EFMigrationsHistory" (migration_id, product_version)
VALUES ('20211214183259_AddIssuesToLocal', '5.0.11');

COMMIT;

