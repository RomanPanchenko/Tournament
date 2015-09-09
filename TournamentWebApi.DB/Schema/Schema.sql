-- DROP SCHEMA sec;

CREATE SCHEMA sec AUTHORIZATION postgres;

-- Sequence: matches_matchid_seq

-- DROP SEQUENCE matches_matchid_seq;

CREATE SEQUENCE matches_matchid_seq
  INCREMENT 1
  MINVALUE 1
  MAXVALUE 9223372036854775807
  START 1
  CACHE 1;
ALTER TABLE matches_matchid_seq
  OWNER TO postgres;

-- Sequence: players_playerid_seq

-- DROP SEQUENCE players_playerid_seq;

CREATE SEQUENCE players_playerid_seq
  INCREMENT 1
  MINVALUE 1
  MAXVALUE 9223372036854775807
  START 21
  CACHE 1;
ALTER TABLE players_playerid_seq
  OWNER TO postgres;


-- Table: players

-- DROP TABLE players;

CREATE TABLE players
(
  playerid serial NOT NULL,
  firstname character varying(50),
  lastname character varying(50),
  registrationtime timestamp without time zone,
  rate integer,
  CONSTRAINT pk_players_playerid PRIMARY KEY (playerid)
)
WITH (
  OIDS=FALSE
);
ALTER TABLE players
  OWNER TO postgres;


-- Table: matches

-- DROP TABLE matches;

CREATE TABLE matches
(
  matchid serial NOT NULL,
  playerid1 integer NOT NULL,
  playerid2 integer NOT NULL,
  winnerid integer NOT NULL,
  matchstarttime timestamp without time zone NOT NULL,
  player1playswhite boolean NOT NULL,
  round integer NOT NULL,
  CONSTRAINT matches_id PRIMARY KEY (matchid),
  CONSTRAINT link_matches_players_playerid1 FOREIGN KEY (playerid1)
      REFERENCES players (playerid) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION,
  CONSTRAINT link_matches_players_playerid2 FOREIGN KEY (playerid2)
      REFERENCES players (playerid) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION,
  CONSTRAINT link_matches_players_winnerid FOREIGN KEY (winnerid)
      REFERENCES players (playerid) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION,
  CONSTRAINT uk_playersinmatch UNIQUE (playerid1, playerid2)
)
WITH (
  OIDS=FALSE
);
ALTER TABLE matches
  OWNER TO postgres;



-- Sequence: sec.accounts_accountid_seq

-- DROP SEQUENCE sec.accounts_accountid_seq;

CREATE SEQUENCE sec.accounts_accountid_seq
  INCREMENT 1
  MINVALUE 1
  MAXVALUE 9223372036854775807
  START 1
  CACHE 1;
ALTER TABLE sec.accounts_accountid_seq
  OWNER TO postgres;


-- Sequence: sec.accountsroleslink_linkid_seq

-- DROP SEQUENCE sec.accountsroleslink_linkid_seq;

CREATE SEQUENCE sec.accountsroleslink_linkid_seq
  INCREMENT 1
  MINVALUE 1
  MAXVALUE 9223372036854775807
  START 1
  CACHE 1;
ALTER TABLE sec.accountsroleslink_linkid_seq
  OWNER TO postgres;


-- Sequence: sec.roles_roleid_seq

-- DROP SEQUENCE sec.roles_roleid_seq;

CREATE SEQUENCE sec.roles_roleid_seq
  INCREMENT 1
  MINVALUE 1
  MAXVALUE 9223372036854775807
  START 1
  CACHE 1;
ALTER TABLE sec.roles_roleid_seq
  OWNER TO postgres;


-- Table: sec.accounts

-- DROP TABLE sec.accounts;

CREATE TABLE sec.accounts
(
  accountid integer NOT NULL DEFAULT nextval('sec.accounts_accountid_seq'::regclass),
  firstname character varying(50),
  lastname character varying(50),
  email character varying(255),
  login character varying(50),
  password character varying(50),
  CONSTRAINT accounts_pkey PRIMARY KEY (accountid)
)
WITH (
  OIDS=FALSE
);
ALTER TABLE sec.accounts
  OWNER TO postgres;


-- Table: sec.roles

-- DROP TABLE sec.roles;

CREATE TABLE sec.roles
(
  roleid integer NOT NULL DEFAULT nextval('sec.roles_roleid_seq'::regclass),
  name character varying(50),
  CONSTRAINT roles_pkey PRIMARY KEY (roleid)
)
WITH (
  OIDS=FALSE
);
ALTER TABLE sec.roles
  OWNER TO postgres;


-- Table: sec.accountsroleslink

-- DROP TABLE sec.accountsroleslink;

CREATE TABLE sec.accountsroleslink
(
  linkid integer NOT NULL DEFAULT nextval('sec.accountsroleslink_linkid_seq'::regclass),
  accountid integer,
  roleid integer,
  CONSTRAINT accounts_roles_link_pkey PRIMARY KEY (linkid),
  CONSTRAINT link_accounts_roles_accountsid FOREIGN KEY (accountid)
      REFERENCES sec.accounts (accountid) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION,
  CONSTRAINT link_accountsroleslink_roles_roleid FOREIGN KEY (roleid)
      REFERENCES sec.roles (roleid) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION
)
WITH (
  OIDS=FALSE
);
ALTER TABLE sec.accountsroleslink
  OWNER TO postgres;

-- Index: sec.fki_link_accountsroleslink_accounts_accountsid

-- DROP INDEX sec.fki_link_accountsroleslink_accounts_accountsid;

CREATE INDEX fki_link_accountsroleslink_accounts_accountsid
  ON sec.accountsroleslink
  USING btree
  (accountid);

-- Index: sec.fki_link_accountsroleslink_roles_roleid

-- DROP INDEX sec.fki_link_accountsroleslink_roles_roleid;

CREATE INDEX fki_link_accountsroleslink_roles_roleid
  ON sec.accountsroleslink
  USING btree
  (roleid);




