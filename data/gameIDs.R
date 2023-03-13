get.ids <- function(seasons) {
  
  require(magrittr)
  
  l.out <- list(length(seasons))
  
  for (i in 1:length(seasons)) {
    l.out[[i]] <- readLines(paste0('https://www.j-archive.com/showseason.php?season=',seasons[i])) %>%
      suppressWarnings %>%
      as.character %>%
      `[`(grep('showgame.php', .)) %>%
      stringr::str_sub(
        start = stringr::str_locate(., 'game_id=')[,2] + 1,
        end = stringr::str_locate(., 'game_id=')[,2] + 4
      ) %>%
      gsub(pattern="[^0-9.-]", replacement="") %>%
      as.integer %>%
      data.frame(
        season = seasons[i],
        game.id = .
      )
  }
  
  out <- do.call(rbind, l.out)
  out
  
}

ids.all <- get.ids(1:39)
