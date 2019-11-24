-- tables
drop table if exists user;
create table user (
	id integer primary key,
	language text not null, -- ISO 639-1
	username text not null unique,
	password_hash text not null,
	password_salt text not null,
	name text not null,
	surname text not null,
	avatar text,
	insert_date datetime not null,
	last_modified datetime not null
);

drop table if exists organization;
create table organization (
	id integer primary key,
	name text not null unique,
	avatar text,
	created_by integer references user,
	insert_date datetime not null,
	last_modified datetime not null
);

drop table if exists user_oranization;
create table user_oranization (
	user_id integer references user on delete cascade,
	organization_id integer references organization on delete cascade,
	access_type integer not null --defined in Enum AccessType (Creator, ReadWrite, Read)
);

drop table if exists note;
create table note (
	id integer primary key,
	content text not null,
	language text not null, -- ISO 639-1
	color integer not null default 0, -- defined in Enum NoteColor
	is_pinned integer not null default 0, -- 0=unpinned; 1=pinned
	is_public integer not null default 0, -- 0=private; 1=public
	is_shared integer not null default 0, -- 0=private; 1=shared
	created_by integer references user on delete cascade,
	insert_date datetime not null,
	last_modified datetime not null
);

drop table if exists user_note;
create table user_note (
	user_id integer references user on delete cascade,
	note_id integer references note on delete cascade,
	access_type integer not null --defined in Enum AccessType (Creator, ReadWrite, Read)
);

drop table if exists organization_note;
create table organization_note (
	organization_id integer references organization on delete cascade,
	note_id integer references note on delete cascade,
	access_type integer not null --defined in Enum AccessType (Creator, ReadWrite, Read)
);
