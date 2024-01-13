import {MdSearch} from "react-icons/md";

function SearchBar() {
    return (
        <div className="flex flex-row items-center relative ml-10">
            <MdSearch className="absolute left-16 text-gray-500 text-2xl"/>
            <input className="border shadow rounded pl-10 py-3 ml-14 w-96"
                type="text" placeholder="Search"
            />
        </div>
    )
}

export default SearchBar