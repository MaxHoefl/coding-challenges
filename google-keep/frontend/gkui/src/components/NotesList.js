import useNotesContext from "../hooks/use-notes-context";
import Note from "./Note";

function NotesList() {
    const { notes } = useNotesContext()
    return (<div className="grid gap-4 m-16">
        {notes.map(note => {
            return <Note key={note.id} note={note}></Note>
        })}
    </div>)
}

export default NotesList