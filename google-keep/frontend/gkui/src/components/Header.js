import {MdOutlineList} from "react-icons/md";
import SearchBar from "./SearchBar";

function Header({ onNavToggle }) {
    return (
        <div className="border-b-2 mb-2">
            <div className="pl-4 flex flex-row items-center">
                <div className="text-3xl flex items-center">
                    <div className="pl-2 pr-1 text-5xl" onClick={onNavToggle}>
                        <MdOutlineList onClick={onNavToggle} className="hover:bg-gray-100 rounded-full p-2"/>
                    </div>
                    <img src="https://static.vecteezy.com/system/resources/previews/027/179/390/original/google-keep-icon-logo-symbol-free-png.png" className="m-2 w-9" alt="Missing icon" />
                    Keep
                </div>
                <SearchBar></SearchBar>
            </div>
        </div>
    )
}

export default Header