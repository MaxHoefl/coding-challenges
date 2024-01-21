import useNotesContext from "../hooks/use-notes-context";
import Note from "./Note";
import {useEffect, useState} from "react";
import NoteEdit from "./NoteEdit";

function NotesList() {
    const { notes, updateNote } = useNotesContext()

    const [selectedNote, setSelectedNote] = useState(null);

    const handleNoteClick = (note) => {
        setSelectedNote(note);
    };

    const handleCloseNoteEdit = () => {
        setSelectedNote(null);
    };

    const handleSaveNote = (updatedNote) => {
        fetch(`http://localhost:8080/notes/${updatedNote.id}`, {
            method: "PUT",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify(updatedNote),
        })
            .then((response) => response.json())
            .then((data) => {
                console.log("Note updated successfully:", data);
                updateNote(data);
            })
            .catch((error) => {
                console.error("Error updating note:", error);
            });
    };

    useEffect(() => {
        const handleKeyDown = (e) => {
            if (e.key === "Escape") {
                handleCloseNoteEdit();
            }
        };

        window.addEventListener("keydown", handleKeyDown);

        return () => {
            window.removeEventListener("keydown", handleKeyDown);
        };
    }, []);


    return (<div className="grid gap-4 m-16">
        {notes.map(note => {
            return <Note key={note.id} note={note} onNoteClick={handleNoteClick}></Note>
        })}
        {selectedNote && <NoteEdit note={selectedNote} onClose={handleCloseNoteEdit} onSave={handleSaveNote}/>}
    </div>)
}

export default NotesList