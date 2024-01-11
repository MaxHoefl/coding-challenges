function NavigationItem({children, onClick, isActive}) {
    let classList = [
        "flex", "items-center",
        "hover:bg-gray-200", "active:bg-yellow-300",
        "pl-9", "py-3",
        "rounded-tr-full", "rounded-br-full", "bg-yellow-300"
    ]
    let classString = isActive ?
        classList.filter(s => s !== "hover:bg-gray-200").join(" ") :
        classList.filter(s => s !== "bg-yellow-300").join(" ")
    return <div onClick={onClick} className={classString}>{children}</div>
}

export default NavigationItem