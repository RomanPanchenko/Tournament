-- Table: Matches

-- DROP TABLE Matches CASCADE;

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