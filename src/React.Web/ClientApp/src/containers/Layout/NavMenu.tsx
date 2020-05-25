import React from "react";
import { useSelector } from "react-redux";
import { Link } from "react-router-dom";
import LoginMenu from "./LoginMenu";

import Nav from "react-bootstrap/Nav";
import Navbar from "react-bootstrap/Navbar";
import NavDropdown from "react-bootstrap/NavDropdown";

import { selectIsAuthenticated, selectUser } from "../../store/slices/auth-slice";
import { selectCategories } from "../../store/slices/category-slice";

const NavMenu = () => {
  const isAuthenticated = useSelector(selectIsAuthenticated);
  const userName = useSelector(selectUser)?.name;
  const categories = useSelector(selectCategories);

  return (
    <header>
      <Navbar collapseOnSelect expand="lg" bg="dark" variant="dark">
        <Navbar.Brand as={Link} to={`/`}>
          BShop
        </Navbar.Brand>
        <Navbar.Toggle aria-controls="responsive-navbar-nav" />
        <Navbar.Collapse id="responsive-navbar-nav">
          <Nav className="mr-auto">
            <NavDropdown title="Dropdown" id="collasible-nav-dropdown">
              {categories.map((category) => (
                <NavDropdown.Item key={category.id} as={Link} to={`/category/${category.id}`}>
                  {category.name}
                </NavDropdown.Item>
              ))}
            </NavDropdown>
          </Nav>
          <LoginMenu isAuthenticated={isAuthenticated} userName={userName} />
        </Navbar.Collapse>
      </Navbar>
      {/* <Navbar color="light" light expand="md">
        <NavbarBrand tag={Link} to="/">
          React.Web
        </NavbarBrand>
        <NavbarToggler onClick={toggle} className="mr-2" />
        <Collapse isOpen={isOpen} navbar>
          <Nav navbar>
            <NavItem>
              <NavLink tag={Link} to="/">
                Home
              </NavLink>
            </NavItem>
            <UncontrolledDropdown nav inNavbar>
              <DropdownToggle nav caret>
                Categories
              </DropdownToggle>
              <DropdownMenu right>
                {categories.map((category) => (
                  <DropdownItem key={category.id} tag={Link} to={`/category/${category.id}`}>{category.name}</DropdownItem>
                ))}
              </DropdownMenu>
            </UncontrolledDropdown>
          </Nav>
          <Nav className="ml-auto" navbar>
            <LoginMenu isAuthenticated={isAuthenticated} userName={userName} />
          </Nav>
        </Collapse>
      </Navbar> */}
    </header>
  );
};

export default NavMenu;
