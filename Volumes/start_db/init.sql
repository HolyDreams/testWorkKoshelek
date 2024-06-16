CREATE SCHEMA IF NOT EXISTS my;
DROP TABLE IF EXISTS my.messages;

CREATE TABLE my.messages (
	id 					uuid 										 	NOT NULL,
	msg_text 			varchar(128) 									NULL,
	msg_date 			timestamp 										NULL,
	created_at 			timestamp 		DEFAULT now() 					NOT NULL,
	modified_at 		timestamp 										NULL,
	delete_state_code 	int2 			DEFAULT 0 						NOT NULL,
	CONSTRAINT messages_pk PRIMARY KEY (id)
);

CREATE OR REPLACE FUNCTION my.row_update() RETURNS TRIGGER as $row_update$
	BEGIN
		NEW.modified_at = now();
		RETURN NEW;
	END;
$row_update$ language plpgsql;

CREATE OR REPLACE FUNCTION my.row_delete() RETURNS TRIGGER as $row_delete$
	BEGIN
		IF (OLD.delete_state_code = 0) THEN
			UPDATE messages
			SET delete_state_code = 1
			WHERE id = OLD.id;
		END IF;
		RETURN NULL;
	END;
$row_delete$ language plpgsql;
	
CREATE OR REPLACE TRIGGER row_update BEFORE UPDATE ON my.messages
	FOR EACH ROW EXECUTE FUNCTION my.row_update();
	
CREATE OR REPLACE TRIGGER row_delete BEFORE DELETE ON my.messages
	FOR EACH ROW EXECUTE FUNCTION my.row_delete();