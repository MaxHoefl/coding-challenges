import {findAllByDisplayValue} from "@testing-library/react";
import {MdLightbulbOutline} from "react-icons/md";

function navItemClassList(isCollapsed, isActive) {
    const bgColorPrimary = "bg-yellow-200"
    const bgColorSecondary = "bg-gray-100"
    let classList = [
        "flex", "items-center",
        "text-md",
        "py-3",
        bgColorPrimary, `hover:${bgColorSecondary}`, `active:${bgColorPrimary}`
    ]
    if (isCollapsed) {
        classList.push("ml-6", "px-3","rounded-full")
    }
    else {
        classList.push("pl-9", bgColorPrimary, `hover:${bgColorSecondary}`, `active:${bgColorPrimary}`, "rounded-tr-full", "rounded-br-full")
    }
    return isActive ?
        classList.filter(s => s !== `hover:${bgColorSecondary}`).join(" ") :
        classList.filter(s => s !== bgColorPrimary).join(" ")
}

function NavigationItem({children, title, onClick, isActive, isCollapsed}) {
    const navTextClass = isCollapsed? "opacity-0": "opacity-100 ml-5"
    return (<div className={navItemClassList(isCollapsed, isActive)} onClick={onClick}>
        {children}
        <span className={`transition-opacity duration-300 ${navTextClass}`}>{isCollapsed? "": title}</span>
    </div>)
}

export default NavigationItem