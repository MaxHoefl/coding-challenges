function Note({ note, onNoteClick }) {
    return (
        <div
            className="border-2 border-gray-200 rounded-lg min-w-64 min-h-32 p-4 hover:shadow-lg hover:border-gray-400"
            onClick={() => onNoteClick(note)}
        >
            <div className="flex flex-col">
                <div className="text-xl font-medium mb-2">{note.title}</div>
                <div>{note.content}</div>
                <div className="max-w-10">

                </div>
            </div>
        </div>
    )
}

export default Note