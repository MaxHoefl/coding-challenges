function Note({ note }) {
    return (
        <div className="border-2 border-gray-200 rounded-lg min-w-64 min-h-32 p-4 hover:shadow-lg hover:border-gray-400">
            <div className="flex flex-col">
                <div className="text-xl font-medium mb-2">{note.title}</div>
                <div>{note.content}</div>
            </div>

        </div>
    )
}

export default Note