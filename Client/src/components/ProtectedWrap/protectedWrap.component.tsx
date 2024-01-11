import { useEffect } from "react";
import { useNavigate } from "react-router-dom";
import TokenService from "../../Services/token.service";

export default function ProtectedWrap(props: { children: JSX.Element }) {
  const navigate = useNavigate();
  useEffect(() => {
    if (!TokenService.isUserLogged()) {
      navigate("/login");
    }
  });

  return <>{props.children}</>;
}
