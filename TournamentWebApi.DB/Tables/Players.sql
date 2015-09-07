-- Table: Players

-- DROP TABLE Players CASCADE;

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