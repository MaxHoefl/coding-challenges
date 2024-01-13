import {useContext} from "react";
import NotesContext from "../context/notes";


function useNotesContext() {
    return useContext(NotesContext)
}

export default useNotesContext