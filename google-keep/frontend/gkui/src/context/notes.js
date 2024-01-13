import {createContext, useEffect, useState} from "react";
import axios from "axios";

const NotesContext = createContext()

function Provider({ children }) {
    const [notes, setNotes] = useState([])
    const serverUrl = "http://localhost:8080"

    const fetchNotes = async () =>  {
        console.log("Fetching notes")
        const response = await axios.get(`${serverUrl}/notes`)
        setNotes(response.data)
    }

    useEffect(() => {
        fetchNotes()
    }, [])

    console.log("-------- Loading note --------")
    console.log("Notes", notes)
    console.log("------------------------------")
    const noteContext = {
        notes
    }
    return (
        <NotesContext.Provider value={noteContext}>
            {children}
        </NotesContext.Provider>
    )
}

export { Provider }
export default NotesContext