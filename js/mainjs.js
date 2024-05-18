setTimeout(function() {
    document.querySelector('.contact-form').classList.add('show');
    setTimeout(function() {
        document.querySelector('.contact-form').classList.add('show');
    }, 1000); // Add a delay of 1s after the contact form is shown
}, 1000);

//ok allah razi olsun
import { BrowserRouter, Routes, Route } from "react-router-dom";
import AnimeItem from "./Components/AnimeItem";
import Gallery from "./Components/Gallery";
import Homepage from "./Components/Homepage";

function App() {
  
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<Homepage />} />
        <Route path="/anime/:id" element={<AnimeItem />} />
        <Route path="/character/:id" element={<Gallery />} />
      </Routes>
    </BrowserRouter>
  );
}
