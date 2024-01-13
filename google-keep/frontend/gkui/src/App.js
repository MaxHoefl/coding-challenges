import NotesPage from "./pages/NotesPage";
import {useEffect, useState} from "react";

function App() {
    const [notes, setNotes] = useState([])

    return <NotesPage></NotesPage>
}

export default App;