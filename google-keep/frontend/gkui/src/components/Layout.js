import Navigation from "./Navigation";
import {useState} from "react";
import Header from "./Header";

function Layout({children}) {
    const [showNavNames, setShowNavNames] = useState(true)

    const handleNavNameToggle = (event) => {
        event.preventDefault()
        setShowNavNames(!showNavNames)
    }

    return (
        <div className="flex flex-col">
            <Header onNavToggle={handleNavNameToggle}></Header>
            <div className="flex flex-row">
                <Navigation isCollapsed={showNavNames}></Navigation>
                <div>{children}</div>
            </div>
        </div>

    )
}

export default Layout