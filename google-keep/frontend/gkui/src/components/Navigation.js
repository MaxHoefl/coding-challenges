import NavigationItem from "./NavigationItem";
import {MdLightbulbOutline, MdOutlineArchive, MdOutlineModeEdit, MdOutlineNotifications} from "react-icons/md";
import {BsTrash} from "react-icons/bs";
import {useState} from "react";

function Navigation() {
    const [activeItem, setActiveItem] = useState("notes")
    const handleItemClick = (itemName) => {
        setActiveItem(itemName)
    }

    return <div className="min-w-64">
        <NavigationItem
                onClick={() => handleItemClick("notes")}
                isActive={activeItem === "notes"}>
            <MdLightbulbOutline className="mr-4 text-xl" />
            Notes
        </NavigationItem>

        <NavigationItem
                onClick={() => handleItemClick("reminders")}
                isActive={activeItem === "reminders"}>
            <MdOutlineNotifications className="mr-4 text-xl" />Reminders
        </NavigationItem>

        <NavigationItem
                onClick={() => handleItemClick("labels")}
                isActive={activeItem === "labels"}>
            <MdOutlineModeEdit className="mr-4 text-xl" />Edit labels
        </NavigationItem>

        <NavigationItem
                onClick={() => handleItemClick("archive")}
                isActive={activeItem === "archive"}>
            <MdOutlineArchive className="mr-4 text-xl" />Archive
        </NavigationItem>

        <NavigationItem
                onClick={() => handleItemClick("bin")}
                isActive={activeItem === "bin"}>
            <BsTrash className="mr-4 text-xl" />Bin
        </NavigationItem>
    </div>
}

export default Navigation