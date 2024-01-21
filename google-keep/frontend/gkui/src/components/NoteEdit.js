import {useState} from "react";

function NoteEdit({ note, onClose, onSave }) {
    const [editedNote, setEditedNote] = useState({ ...note });

    const handleInputChange = (e) => {
        const { name, value } = e.target;
        setEditedNote((prevNote) => ({ ...prevNote, [name]: value }));
    };

    const handleSave = () => {
        onSave(editedNote);
        onClose();
    };

    return (
        <div className="fixed top-0 left-0 w-full h-full flex items-center justify-center bg-black bg-opacity-50">
            <div className="bg-white p-4 rounded-lg">
                <input
                    type="text"
                    name="title"
                    value={editedNote.title}
                    onChange={handleInputChange}
                    className="text-xl font-medium mb-2 w-full border-2 border-gray-200 p-2"
                />
                <textarea
                    name="content"
                    value={editedNote.content}
                    onChange={handleInputChange}
                    className="w-full h-32 border-2 border-gray-200 p-2"
                ></textarea>
                <button
                    className="mt-2 bg-yellow-200 text-black px-4 py-2 rounded"
                    onClick={handleSave}
                >
                    Save
                </button>
                <button className="mt-2 ml-2 px-4 py-2 rounded" onClick={onClose}>
                    Close
                </button>
            </div>
        </div>
    );
}


export default NoteEdit