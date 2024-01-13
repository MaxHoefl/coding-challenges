import NavigationItem from "./NavigationItem";
import {MdLightbulbOutline, MdOutlineArchive, MdOutlineModeEdit, MdOutlineNotifications} from "react-icons/md";
import {useState} from "react";
import {BsTrash} from "react-icons/bs";

function navClassList(isCollapsed) {
    const classList = ["transition-all", "duration-100", "ease-in-out"]
    if (isCollapsed) {
        classList.push("min-w-12")
    }
    else {
        classList.push("min-w-64")
    }
    return classList.join(" ")
}

function Navigation({ isCollapsed }) {
    const [activeItem, setActiveItem] = useState("notes")
    const handleItemClick = (itemName) => {
        setActiveItem(itemName)
    }
    return <div className={navClassList(isCollapsed)}>
        <NavigationItem
            isCollapsed={isCollapsed}
            isActive={activeItem === "notes"}
            onClick={() => handleItemClick("notes")}
            title="Notes">
            <MdLightbulbOutline className="text-2xl" />
        </NavigationItem>

        <NavigationItem
            isCollapsed={isCollapsed}
            isActive={activeItem === "reminders"}
            onClick={() => handleItemClick("reminders")}
            title="Reminders">
            <MdOutlineNotifications className="text-2xl" />
        </NavigationItem>

        <NavigationItem
            isCollapsed={isCollapsed}
            isActive={activeItem === "labels"}
            onClick={() => handleItemClick("labels")}
            title="Labels">
            <MdOutlineModeEdit className="text-2xl" />
        </NavigationItem>

        <NavigationItem
            isCollapsed={isCollapsed}
            isActive={activeItem === "archive"}
            onClick={() => handleItemClick("archive")}
            title="Archive">
            <MdOutlineArchive className="text-2xl" />
        </NavigationItem>

        <NavigationItem
            isCollapsed={isCollapsed}
            isActive={activeItem === "bin"}
            onClick={() => handleItemClick("bin")}
            title="Bin">
            <BsTrash className="text-2xl" />
        </NavigationItem>
    </div>
}

export default Navigation