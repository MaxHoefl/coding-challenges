import Navigation from "./Navigation";

function Layout({children}) {
    return (
        <div className="flex flex-row">
            <Navigation></Navigation>
            <div>{children}</div>
        </div>

    )
}

export default Layout